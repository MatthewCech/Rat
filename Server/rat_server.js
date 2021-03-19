const express = require('express');
const app = express();
const util = require('util');
const request = require('request');

var cors = require('cors');

// General config
const api_port = 8080;                      
const api_separator = "/";                  
const api_forward = api_separator + "api/"; 
const api_root = api_forward + "v1/";       
const api_rat = api_root + "rat/";

// Confiure coors
app.use(cors());

// Format a successful generic response
function formatSuccess(optionalNote) {
	obj = {};
	obj.server = serverName;
	obj.status = `success`;

	if(optionalNote)
		obj.note = optionalNote;

	return obj;
}

  //////////////////////
 // Highscore server //
//////////////////////

// Highscore storage. Note that without proper saving,
// this gets destroyed with every server restart or crash. By default,
// we'll have some common benchmark highscores in here.
let highscores = [
	{
		tag:"rat",
		fg:"36.22.12",
		bg:"143.97.61",
		score:180
	},
	{
		tag:"rat",
		fg:"36.22.12",
		bg:"143.97.61",
		score:60
	},
	{
		tag:"rat",
		fg:"36.22.12",
		bg:"143.97.61",
		score:120
	},
	{
		tag:"rat",
		fg:"36.22.12",
		bg:"143.97.61",
		score:30
	}
];

app.all(api_rat, (req, res) => {
	res.send(formatSuccess("Rat leaderboard prototype reached!"))
});

// <site>/api/v1/rat/add?tag=sampleTag&fg=someValue&bg=otherValue&score=123
app.all(api_rat + "add", (req, res) => {
	let query_tag = req.param("tag");
	let query_fg = req.param("fg");
	let query_bg = req.param("bg");
	let query_score = req.param("score");

	if(query_tag.length > 2 && query_tag != "rat")
	{
		query_tag = query_tag.substring(0, 2)
	}

	highscores.push({tag:query_tag, fg:query_fg, bg:query_bg, score:query_score});

	res.send(formatSuccess(`Got back tag ${query_tag}, fg ${query_fg}, bg ${query_bg}, score ${query_score}`));
});

// Returns all highscore data
app.all(api_rat + "dump", (req, res) => {
	res.send({entries:highscores});
});


  ////////////////////
 // Tell it to run //
////////////////////

// Bind to port and begin listening
app.listen(api_port, () => {
	GlobalLog.log(`API active, listening on port ${api_port}`)
});
