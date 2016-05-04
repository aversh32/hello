var http = require("https");
var oauthSignature = require("oauth-signature");
var fs=require('fs');
var utf8 = require('utf8');
var querystring=require('querystring');
var date=new Date;
NONCE_CHARS= ['a','b','c','d','e','f','g','h','i','j','k','l','m','n',
              'o','p','q','r','s','t','u','v','w','x','y','z','A','B',
              'C','D','E','F','G','H','I','J','K','L','M','N','O','P',
              'Q','R','S','T','U','V','W','X','Y','Z','0','1','2','3',
              '4','5','6','7','8','9'];

function getNonce(nonceSize) {
   var result = [];
   var chars= NONCE_CHARS;
   var char_pos;
   var nonce_chars_length= chars.length;

   for (var i = 0; i < nonceSize; i++) {
       char_pos= Math.floor(Math.random() * nonce_chars_length);
       result[i]=  chars[char_pos];
   }
   return result.join('');
}
var getTimestamp= function() {
  return Math.floor( (new Date()).getTime() / 1000 );
}

function find(response,data)
{	
	//data = encodeURI(data);
 var startTime = +new Date();
	//var utf8 = require('utf8');
	var Method = "GET";
	var url = "https://api.twitter.com/1.1/search/tweets.json";
	var parameters = {
		q:data,
		count:"100",
		result_type:"popular",
		oauth_version:"1.0",
		oauth_consumer_key:"GAtR5r1yXAUVDbBQZZeEW4wHO", 
		oauth_token:"2868760687-tQ6aOn29d343R7rKpdasgSGbv2aAKGsf4I7RYHp", 
		oauth_timestamp:"1461843633", 
		oauth_nonce:"czMlz2tiX4E", 		
		oauth_signature_method:"HMAC-SHA1", 						
	};
	var transcode
	{
		encodeSignature: true
	};
	var consumerSecret = "vqMcaTrLGwhPPgAA7XXiJU6kRbY7sfEwXfcwCX9Vch0jV9XErm";
	var tokenSecret = "FxEtUMtysAlzHDkxKI2JHdbXwCctc1jGMBEMbGsVxhJ0B";
	var signatureBaseString = new oauthSignature.SignatureBaseString(Method, url, parameters).generate();
	var encodedSignature = oauthSignature.generate(Method, url, parameters, consumerSecret, tokenSecret, transcode);
	console.log(signatureBaseString);
	console.log(encodedSignature);	 
	console.log(encodeURI(data));
	parameters.oauth_signature = encodedSignature;
	parameters.q=encodeURI(data);
	var reqs='OAuth ';
		for(param in parameters)
		{
			reqs+=param+'=\"'+parameters[param]+'\",';			
		}
	var options = {
	//key: fs.readFileSync('lib/key.pem'),
  method: "GET",
  hostname: "api.twitter.com",
  port: null,
  path: "/1.1/search/tweets.json?q="+encodeURI(data)+"&count=100&result_type=popular",
  headers: {Authorization: reqs}
};
		  var req = http.request(options, function (res) {
			 
			  var chunks = [];
			  res.on("data", function (chunk) {
				chunks.push(chunk);				 
		  });
		  res.on("end", function () {
			  console.log(options.path);
			  console.log(options.headers.Authorization);
			var body = Buffer.concat(chunks);
					
			if(body.length>0)
			{
				console.log(body);	
				var tweet = JSON.parse(body);	
				console.log("tweet="+tweet);	
				var jso = JSON.stringify(tweet);
				console.log("jso="+jso);	
				var t = JSON.parse(jso);
				console.log("t="+t);
				//console.log(t.statuses[0]);
				if(t.statuses[0]!=null) {
					t.statuses.forEach(function(item,i,arr){
					console.log("1");
					var t1=JSON.stringify(item);
					console.log("t1="+t1);
					var t11 = JSON.parse(t1);
					console.log("t11="+t11);
					response.write("<br>twitt by "+t11.user.screen_name+":   "+t11.text+"  retweets ="+t11.retweet_count+"<br>", function(err){response.end();});			
					});
				}
				else {response.write("<br>Empty result"); req.end(); response.end(); return;}
			}
			else {response.write("<br>Empty result"); req.end(); response.end(); return;}
		  });
		 
		});	
		  console.log(+new Date()-startTime);
		req.end();		
}

exports.find=find;
