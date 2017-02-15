/**
 * The client side control of the people finder. Written by Ed&Josh.
 */

//var SERVER = 'http://app0111laka01.iaf/PplFndr/api/values/';
var SERVER = 'http://localhost:34781/api/values/';

angular.module('phoneLocator',['ngMaterial', 'ngAnimate', 'ngAria']);

angular.module('phoneLocator').controller('FinderCtrl', function($scope, phoneService, httpExtension) {
    var MINIMUM_QUERY_LENGTH = 3;
    window.phoneLocatorScope = $scope;

    chrome.storage.sync.get(['pinnedPersons'], function(persons){ 
        if(persons['pinnedPersons'])
            window.phoneLocatorScope.peopleToEmail = persons['pinnedPersons'];
        else
            window.phoneLocatorScope.peopleToEmail = [];
    });
	
	$scope.isInBotMode = false;

    $scope.peopleToEmail = [];

    $scope.AppStates = {
        DEFAULT: 0,
        SHOW_PEOPLE: 1,
        NOT_ENOUGH_CHARACTERS: 2,
        LOADING: 3,
        ERROR: 4
    };
    $scope.appState = $scope.AppStates.DEFAULT;

    // We would like to send a query every time the text changes but as this creates significant
     // overhead, we throttle them by only sending queries for ~1/8 text changes.
     // Additionally, if the user has stopped typing, we also want to send a query as throttling
     // is less of a concern if there is some gap between keystrokes.
     // To accomplish the latter, we set this timeout variable after every text change and cancel
     // the previous ones so that there is only one active timeout function tracking text updates
     // at a time.
     var sendQueryTimeout;
     $scope.onTextChange = function(opt_forceNewRequest) {
        // We throttle by using a random number generator. Note that if the query is too short,
        // we handle it right away because we already know what to do with it.
        if (Math.random() * 8 >= 7 || isQueryTooShort()) {
            maybeDoQuery(opt_forceNewRequest);
        } else {
            clearSendQueryTimeout();
            sendQueryTimeout = setTimeout(function() {
                maybeDoQuery(opt_forceNewRequest);
            }, 500);
        }
     };

     // When submitting queries is manually triggered.
     $scope.submitQuery = function(opt_forceNewRequest) {
         maybeDoQuery(opt_forceNewRequest);
     };

     // A helper function to actually make the query.
     function maybeDoQuery(opt_forceNewRequest) {
        var queryTooShort = isQueryTooShort();
        if (queryTooShort) {
            $scope.appState = $scope.AppStates.NOT_ENOUGH_CHARACTERS;
            return;
        }

        var query = $scope.query;

        // Don't make the same request twice.
        if (query == currentRequestQuery && !opt_forceNewRequest) {
            return;
        }

        if (opt_forceNewRequest) {
            logRequest(LOG_TYPES.SEE_ALL_PRESSED);
        }

        // We also cancel the previous timeout every time we make a query as an added safety
        // mechanism (e.g., if the user presses enter to send a request).
        clearSendQueryTimeout();

        // TODO(josh): Use ng-animate to make this more effective.
        $scope.appState = $scope.AppStates.LOADING;

        currentRequestQuery = $scope.query;
        phoneService.get($scope.query, opt_forceNewRequest)
            .success(function(data) {
                handleResponse(data, query);
            })
            .error(function() {
                $scope.appState = $scope.AppStates.ERROR;
            });
        logRequest(LOG_TYPES.REQUEST);
     };

     // A global variable that tracks the query that was most recently sent. This makes sure that if
     // a second query was sent before the first one returns, we abandon the first response and stick
     // with the second.
     var currentRequestQuery;
     function handleResponse(data, query) {
        $scope.appState = $scope.AppStates.SHOW_PEOPLE;
        // The first item in data should be the metadata object. Everything else is people.
        $scope.metadata = data.splice(0,1)[0];
        $scope.people = data;
     };

     function clearSendQueryTimeout() {
        clearTimeout(sendQueryTimeout);
     };

     function isQueryTooShort() {
		 return isGivenQueryTooShort($scope.query);
     }

     // A valid query must contain at least one space-separated word that is at least
     // MINIMUM_QUERY_LENGTH characters long.
     function isGivenQueryTooShort(query) {
        var words = query.split(' ');
        for (var i = 0; i < words.length; i++) {
            if (words[i].length >= MINIMUM_QUERY_LENGTH) {
                return false;
            }
        }
        return true;
     }


     // If the user taps on an email address field, close the extension and redirect to an email URL.
     $scope.sendEmail = function(email) {
        logRequest(LOG_TYPES.MAIL);
        var emailUrl = 'mailto:' + email;
        chrome.tabs.create({ url: emailUrl, active: false }, function (tab) {
            setTimeout(function() {
                chrome.tabs.remove(tab.id);
            }, 500);
        });
		//sendEmail(email);
     };

	 /*function sendEmail(email){
  try{
    var theApp = new ActiveXObject("Outlook.Application");
    var objNS = theApp.GetNameSpace('MAPI');
    var theMailItem = theApp.CreateItem(0); // value 0 = MailItem
    theMailItem.to = email;
    //theMailItem.Subject = ('test');
    //theMailItem.Body = ('test');
    //theMailItem.Attachments.Add("C\\file.txt");
    theMailItem.display();
   }
    catch (err) {
       alert(err.message);
    } 
}*/
	 
     $scope.sendMailToPeople = function(persons) {
        var emails = '';
        for (var i = 0; i < persons.length; i++) {
            emails += persons[i].mail+';';
        }
        $scope.sendEmail(emails);
        $scope.clearPinned();
     };

     $scope.showSendMailToAllButton = function() {
        return $scope.people.length > 1
            && $scope.people.length < MAX_NUMBER_OF_EMAILS_IN_URL;
     }

     $scope.togglePinPerson = function(person) {
        if ($scope.isPinned(person)) {
            $scope.unpin(person);
        } else {
            pinPerson(person);
        }
     };

     $scope.clearPinned = function() {
        $scope.peopleToEmail = [];
        updatePinnedPersons();
     };

     function pinPerson(person) {
        if ($scope.peopleToEmail.length > MAX_NUMBER_OF_EMAILS_IN_URL) {
            alert('יותר מדי :(');
            return;
        }
        $scope.peopleToEmail.push(person);
        updatePinnedPersons();
     }

     $scope.isPinned = function(person) {
        for (var i = 0; i < $scope.peopleToEmail.length; i++) {
            if ($scope.peopleToEmail[i].mail == person.mail) {
                return true;
            }
        }
        return false;
     }

     $scope.unpin = function(person) {
        var emailList = $scope.peopleToEmail.map(function(p){
            return p.mail;
        });
        var indexOfPerson = emailList.indexOf(person.mail);
        $scope.peopleToEmail.splice(indexOfPerson, 1);
        updatePinnedPersons();
     };

     $scope.setWhatIDoMessage = function(person) {
        var whatIDoToAdd = person.whatIDoToAdd;
        var postObject = createRequestObject('whatido', whatIDoToAdd);
        postObject.MisparIshi = person.mispar_ishi;
        httpExtension.sendPost(postObject).then(function() {
            person.what_i_do = whatIDoToAdd;
        });
        person.editing_what_i_do = false;
        // TODO(Josh): Is this a hack?
        person.what_i_do = 'נטען...';
     };

     function updatePinnedPersons() {
        chrome.storage.sync.set({'pinnedPersons':$scope.peopleToEmail});
     }

     $scope.onMorePressed = function(person) {
        person.showMore = !person.showMore;
        logRequest(LOG_TYPES.MORE_PRESSED);
     };


     $scope.searchForTag = function(tag) {
        $scope.query = '#' + tag;
        $scope.onTextChange(true /* opt_forceNewRequest */);
     };


     $scope.submitTag = function() {
        var requestObject = createRequestObject('adminaddtag', $scope.tagToAdd);
        requestObject.TagForAnyone = !!$scope.tagToAddIsForAnyone;
        httpExtension.sendGet(requestObject).success(function(data) {
            if (!data.length || !data[0].response) {
                return;
            }
            $scope.tagAddedMessage = data[0].response;
        });
     };

     // TODO(Josh): Make the following two functions smart by sorting the lists first.
     $scope.getTagsForPerson = function(person) {
        person.hide_show_more_tags = true;
        person.tagOptions = [];
        for (var i = 0; i < allTags.length; i++) {
            var tagWrapper = allTags[i];
            if (!personHasTag(person.tags, tagWrapper.tag)) {
                person.tagOptions.push(tagWrapper.tag);
            }
        }
     };

     function personHasTag(personTags, tag) {
        for (var i = 0; i < personTags.length; i++) {
            if (personTags[i].tag == tag) {
                return true;
            }
        }
        return false;
     };

     $scope.tagDropdownSelected = function(person) {
        var newTag = person.newSelectedTag;
        person.newSelectedTag = '';
        if (!newTag) {
            return;
        }

        var requestObject = createRequestObject('addtag', newTag);
        requestObject.MisparIshi = person.mispar_ishi;
        httpExtension.sendGet(requestObject).success(function(data) {
            if (!data.length || !data[0].new_tag) {
                return;
            }
            person.tags.push(data[0].new_tag);
            $scope.getTagsForPerson(person);
        });
     };

     $scope.deleteTagForPerson = function(person, tagToDelete) {
        var postObject = createRequestObject('deletetag', tagToDelete.tag);
        postObject.MisparIshi = person.mispar_ishi;
        httpExtension.sendPost(postObject).success(function(data) {
            var indexOfTagToDelete = person.tags.indexOf(tagToDelete);
            if (indexOfTagToDelete != -1) {
                person.tags.splice(indexOfTagToDelete, 1);
            }
            $scope.getTagsForPerson(person);
        });
     };

     $scope.canAddTags = function(person) {
        return (person.is_me && $scope.metadata.nonAdminsCanAddTags) || $scope.isAdmin;
     };

     $scope.canEditWhatDoIDo = function(person) {
        return person.is_me || $scope.isAdmin;
     };

     $scope.canDeleteTag = function(person, tag) {
        return $scope.isAdmin || (person.is_me && tag.non_admins_can_edit);
     };

    // TODO(Josh): Move this to a module or something like that.
    var LOG_TYPES = {
        INITIAL: 'initial',
        MAIL: 'mail',
        REQUEST: 'request',
        MORE_PRESSED: 'morepressed',
        SEE_ALL_PRESSED: 'seeallpressed',
        TIMER: 'timer'
    };

    function logRequest(logType) {
        var postObject = createRequestObject('log');
        postObject.Logs = {
            SessionId: SESSION_ID,
            LogType: logType
        };
        var logString = SESSION_ID + ',' + logType;
        if (logType == LOG_TYPES.REQUEST) {
            postObject.Logs.Query = $scope.query;
        }

        httpExtension.sendPost(postObject);
    }

    var allTags;
    httpExtension.sendGet(createRequestObject('initialmetadata')).success(function(data) {
        if (!data.length) {
            return;
        }
        var metadata = data[0];
        $scope.isAdmin = metadata.is_admin;

        allTags = metadata.tags_to_add;
    });


    logRequest(LOG_TYPES.INITIAL);
	
	
	// Chats is an array of objects of the form
	// { message: "Hello world", is_bot: true/false, persons: person }
	$scope.chats = [];
	
	$scope.isInBotMode = true;
	$scope.chats.push(createChatObject('שלום, את מי מחפשים?', true));

	$scope.submitChat = function() {
		var chat = $scope.chat_input;
		$scope.chat_input = '';
		$scope.chats.push(createChatObject(chat, false));
		
		var requestObject = createRequestObject('message', chat);
		requestObject.ShowAll = true;
		if (isGivenQueryTooShort(chat)) {
			$scope.chats.push(createChatObject('נא להקליד יותר תווים', true));
			return;
		}
		
		$scope.bot_is_typing = true;
		httpExtension.sendGet(createRequestObject('message', chat)).success(function(data) {
			$scope.bot_is_typing = false;


			var metadata = data.splice(0,1)[0];
			var queryMessage = '';
			if(data.length == 0) {
				queryMessage = "מצטער, לא נמצאו תוצאות...";
			}
			else {
				queryMessage = (metadata && metadata.templateData && metadata.templateData.query) || '';
			}
			
	
			var responseChat = createChatObject(queryMessage, true);
			responseChat.persons = data;
			
			responseChat.action = metadata.templateData.action;
			
			$scope.chats.push(responseChat);
			
			if (metadata && metadata.shouldShowSeeMore) {
			//	$scope.chats.push({ is_bot: true, message: 'יש יותר תוצאות, להציג את כולם?'});
			}

		});
	};
	
	function createChatObject(message, isBot) {
		var now = new Date();
		var timeString = pad(now.getHours(), 2) + ':' + pad(now.getMinutes(), 2);
		return {
			is_bot: isBot,
			message: message,
			time: timeString
			};
	}
	
	function pad(num, size) {
		var s = num+"";
		while (s.length < size) s = "0" + s;
		return s;
	}
	

    // TODO(Josh&Ed): This seems pretty bad.
   //  window.setInterval(function() {logRequest(LOG_TYPES.TIMER)}, 1000);
});

angular.module('phoneLocator').service('phoneService', function($http, httpExtension) {
    return {
        // Already assumes that data validation has been done on query (e.g., for length).
        get: function(query, opt_showAll) {
            var request = createRequestObject('query', query);
            request.ShowAll =  !!opt_showAll;
            return httpExtension.sendGet(request);
        }
    };
});

angular.module('phoneLocator').service('httpExtension', function($http) {
    return {
        sendPost: function(requestObject) {
            // TODO(josh): Do we need this '1'?
            return $http.post(SERVER + '1', requestObject);
        },

        sendGet: function(requestObject) {
            return $http({
                url: SERVER,
                method: 'GET',
                params: requestObject
            });
        }
    };
});

function createRequestObject(type, opt_data) {
    return {
        Type: type,
        Data: opt_data,
    };
};

var PROBLEMATIC_CHARACTERS = ['.', '+', ':', '%'];

var SESSION_ID = Date.now() + Math.random();

var MAX_NUMBER_OF_EMAILS_IN_URL = 100;

/*hackathon adding*/
angular.module("phoneLocator").controller("menuPopupCtrl", function($scope) {
	this.isOpen = false;
	this.mode = 'md-fling';
	this.direction = 'left';
	
	$scope.sendEmail = function(email) {
        var emailUrl = 'mailto:' + email;
        chrome.tabs.create({ url: emailUrl, active: false }, function (tab) {
            setTimeout(function() {
                chrome.tabs.remove(tab.id);
            }, 500);
        });
		//sendEmail(email);
     };
	
	$scope.skypeChat = function(userName) {
		var user = 'itay.fichman';
		var userTemp = "s" + userName;
        var skypeUrl = 'skype:' + user + "?chat";
        chrome.tabs.create({ url: skypeUrl, active: false }, function (tab) {
            setTimeout(function() {
                chrome.tabs.remove(tab.id);
            }, 500);
        });
     };
	 
	 $scope.skypeCall = function(userName) {
		var user = 'itay.fichman';
		var userTemp = "s" + userName;
        var skypeUrl = 'skype:' + user + "?call";
        chrome.tabs.create({ url: skypeUrl, active: false }, function (tab) {
            setTimeout(function() {
                chrome.tabs.remove(tab.id);
            }, 500);
        });
     };
});
/*hackathon adding*/
