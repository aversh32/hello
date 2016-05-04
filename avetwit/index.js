var server = require("./server");
var router = require("./router");
var auth = require("./auth");
var requestHandlers = require("./requestHandlers");

var handle = {}
handle["/"] = requestHandlers.start;
handle["/search"] = requestHandlers.search;
server.start(router.route, handle);
