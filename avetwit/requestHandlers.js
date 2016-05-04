var querystring = require("querystring");
var check = require("./check");
var utf8 = require('utf8');
function start(response, postData) {
  var body = '<html>'+
    '<head>'+
    '<meta http-equiv="Content-Type" content="text/html; '+
    'charset=UTF-8" />'+
    '</head>'+
    '<body>'+
    '<form action="/search" method="post">'+
    '<textarea name="text" rows="2" cols="60"></textarea>'+
    '<input type="submit" value="Search" />'+
    '</form>'+
    '</body>'+
    '</html>';
                        response.statusCode = 200;  // OK
						 response.write(body);
                        response.end();
}

function search(response, postData) {
  console.log("Request handler 'search' was called."); 
  response.writeHead(200, {"Content-Type": "text/html; charset=utf-8" });
  var data  = querystring.parse(postData).text;
  var body = '<html>'+
    '<head>'+
    '<meta http-equiv="Content-Type" content="text/html; '+
    'charset=UTF-8" />'+
    '</head>'+
    '<body>'+
    '<form action="/search" method="post">'+
    '<textarea name="text" rows="2" cols="60"></textarea>'+
    '<input type="submit" value="Search" />'+
    '</form>'+
    '</body>'+
    '</html>';
  response.write(body);		
	//response.removeHeader('Content-type');
	//response.setHeader("Content-Type", "text/plain");
  response.write("You've sent: " +data);
  check.find(response, data);

  //response.end();
}
exports.start = start;
exports.search = search;