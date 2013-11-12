/*==========================================================================;
 *
 *  (...)
 *
 *  File:    First.js
 *  Desc:    Client-side code
 *  Created: Oct-2011
 *
 *  Author:  Miha Grcar
 *
 ***************************************************************************/
        
if (first == null)
{    
	var first = function() {	    

        var images = new Array();
        
        function preloadImages(urlList) 
        {
	        for (var i = 0; i < urlList.length; i++)
	        {
	            var img = new Image();
	            img.src = urlList[i];
	            images.push(img);
	        }
	    }

		function newGuid()
		{
			return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
				var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
				return v.toString(16);
			}).toUpperCase();
		}

		return {		
			addToOnLoad : function(func) {
				var existingOnLoad = window.onload;
				if (existingOnLoad)
				{
					window.onload = function() { existingOnLoad(); func(); };
				}
				else
				{
					window.onload = func;
				}
			},
			addToOnBeforeUnload : function(func) {
				var existingOnBeforeUnload = window.onbeforeunload;
				if (existingOnBeforeUnload)
				{
					window.onbeforeunload = function() { existingOnBeforeUnload(); func(); };
				}
				else
				{
					window.onbeforeunload = func;
				}
			},
		    preloadButtonImages : function() {
			    preloadImages([
			        "http://first.ijs.si/Demos/Common/Css/Img/btn_bg.png",
			        "http://first.ijs.si/Demos/Common/Css/Img/btn_left.png",
			        "http://first.ijs.si/Demos/Common/Css/Img/btn_right.png",
			        "http://first.ijs.si/Demos/Common/Css/Img/btn_bg_hover.png",
			        "http://first.ijs.si/Demos/Common/Css/Img/btn_left_hover.png",
			        "http://first.ijs.si/Demos/Common/Css/Img/btn_right_hover.png",
					"http://first.ijs.si/Demos/Common/Img/Loading.gif"
			    ]);
		    },
			init : function() {
				first.preloadButtonImages();
			},
			htmlEncode : function(str) {
				return str
					.replace(/&/g, '&amp;')
					.replace(/"/g, '&quot;')
					.replace(/'/g, '&#39;')
					.replace(/</g, '&lt;')
					.replace(/>/g, '&gt;');
			},
			ajax : function(url, responseHandler) {
				first.processAjax(url, responseHandler);
			},
			processAjax : function(url, responseHandler) {
				var xmlHttp;
				// prevent caching 
				if (url.indexOf("?") != -1)
				{
					url += "&" + newGuid();
				}
				else
				{
					url += "?" + newGuid();
				}
				try
				{
					// Firefox, Opera 8.0+, Safari
					xmlHttp = new XMLHttpRequest();
				}
				catch (e)
				{
					// Internet Explorer
					try
					{
						xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
					}
					catch (e)
					{
						try
						{
							xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
						}
						catch (e)
						{
							alert("Your browser does not support AJAX!");
							return;
						}
					}
				}
				xmlHttp.onreadystatechange = function() {
					if (xmlHttp.readyState == 4)
					{
						responseHandler(xmlHttp.status, xmlHttp.responseText);
					}
				};
				xmlHttp.open("GET", url, true);
				xmlHttp.send(null);
			}
       };
    }();
}