var http = require("http");
var express = require("express");
var request=require('request');
var bodyParser = require("body-parser");
var templating = require('consolidate');
var qs = require('querystring');
/*http.createServer(function(req,res){
	
}).listen(8000);*/
app = express();
app.engine('hbs', templating.handlebars);
app.set('view engine', 'hbs');
var deals = [];

function addDeal(req,res){
	var body = '';
	req.setEncoding('utf8');
	req.on('data', function(chunk){ body += chunk });
	req.on('end', function(){
		var obj = qs.parse(body);
		deals.push(obj.item);
		//show(res);
		console.log(deals);
		//console.log(deals[0]);
	});
}

app.use(bodyParser.json());
app.set('views', __dirname + '/views');
app.get('/', function(req,res){
	res.render("main");
});

app.post('/click', function(req,res){
	addDeal(req,res);
	res.render("main",{deal:deals});
	console.log(req.value);
	console.log(res.value);
		console.log(req);
	console.log(res);
} );


app.post('/add', function(req,res){
	addDeal(req,res);
	res.render("main",{deal:deals});
});

app.get('/edit', function(req,res){
	
});

app.get('/delete', function(req,res){
	
});

app.listen(8000);
console.log("Server is start");