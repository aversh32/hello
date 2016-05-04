var http = require('http');
var requestHandlers = require("./requestHandlers");
var url = require("url");
function start(route, handle) {
  function onRequest(req, res) {
        var auth = req.headers['authorization'];  
        console.log("Authorization Header is: ", auth);
        if(!auth) {     
                res.statusCode = 401;
                res.setHeader('WWW-Authenticate', 'Basic realm="Secure Area"');
                res.end('<html><body>Need some creds son</body></html>');
        }

        else if(auth) {    
                var tmp = auth.split(' ');   
                var buf = new Buffer(tmp[1], 'base64'); 
                var plain_auth = buf.toString();        
                console.log("Decoded Authorization ", plain_auth);
  
                var creds = plain_auth.split(':');  
                var username = creds[0];
                var password = creds[1];
                if((username == 'aversh32') && (password == '211095')) {   
					var pathname = url.parse(req.url).pathname;
					req.setEncoding("utf8");
					var postData="";
					req.addListener("data", function(postDataChunk) {
						  postData += postDataChunk;
						  console.log("Received POST data chunk '" + postDataChunk + "'.");						  
					});

					req.addListener("end", function() {
						route(handle, pathname, res, postData);
					});		
                }
                else {
                        res.statusCode = 401; 
                        res.setHeader('WWW-Authenticate', 'Basic realm="Secure Area"');                        
						res.end('<html><body>You shall not pass</body></html>');
                }
        }

  }
  http.createServer(onRequest).listen(5000);
  console.log("Server has started.");
}

exports.start = start;