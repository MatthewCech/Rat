const express = require("express");
const app = express();
const util = require("util");
const request = require("request");
const cors = require("cors");



// General config
const api_port = 38080;
const version = 1;
const serverName = "rat_1";

// Convenience. Everything is located at /v#/<stuff>
const api_separator = "/";
const api_root = api_separator + `v${version}/`;



  ///////////////////////
 // General functions //
///////////////////////

function formatBase(status, note) {
	obj = {};
	obj.server = serverName;
	obj.status = status;

	if(note)
		obj.note = note;

	return obj;
}

// Format a successful generic response
function formatSuccess(optionalNote) {
	let res = formatBase("success", optionalNote);
	GlobalLog.log(`Success: ${JSON.stringify(res)}`);
	return res;
}

// Format a failed generic response
function formatError(optionalNote) {
	let err = formatBase("error", optionalNote);
	GlobalLog.error(`Error: ${JSON.stringify(err)}`);
	return err;
}

// Logging
GlobalLog = {};
GlobalLog.log = (toLog) => { console.log(`[Log] ${toLog}`); }
GlobalLog.warn = (toLog) => { console.log(`[Warning] ${toLog}`) }
GlobalLog.error = (toLog) => { console.log(`[ERROR] ${toLog}`) }



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

// Confiure coors
app.use(cors());

// General responses
app.all(api_separator, (req, res) => {
	res.send(formatSuccess("Please indicate version: For example, /v1/"));
});

// Handle root. Respond with version.
app.all(api_root, (req, res) => {
	res.send(formatSuccess(`You've reached version ${version}`));
});

// Sample entry for [Tag: ab, Fg: 127.128.129, Bg: 15.16.17, Score: 123]
// /add?entry=iyd9IaCH%5c%40R%5b%3aPd9Le9Me9Nd9I%5d
app.all(api_root + "add", (req, res) => {
	try {
		let entry = req.param("entry")

		if(entry === undefined) {
			res.send(formatError("Received add, but didn't have expecetd param(s)."));
		} else {

			let constructed = "";
			let isValid = true;
			GlobalLog.log(`Received raw entry: ${entry}`);

			for (let i = 0; i < entry.length; i++) {
				// Setup
				const shift = "Rat";
				const charHigh = "z".charCodeAt(0);
				const charLow = "0".charCodeAt(0);
				const diff = charHigh - charLow;

				// Pre
				let char = "" + entry[i];
				if (char == " ") {
					char = "=";
				}

				// Shift
				let val = char.charCodeAt(0) - shift.charCodeAt(i % shift.length); 
				while (val < charLow) {
					val += diff;
				}
				while (val > charHigh) {
					val -= diff;
				}

				// Post	
				let post = String.fromCharCode(val);
				if (post == ";") {
					post = ".";
				}

				// Append
				constructed += post;
			}

			const sections = constructed.split(":");

			// Verify number of sections
			if(sections.length != 4) {
				isValid = false;
			}

			if(!isValid) {
				res.send(formatError("Received add, but was improperly formatted."));
			} else {
				let query_tag = sections[0];
				let query_fg = sections[1];
				let query_bg = sections[2];
				let query_score = parseInt(sections[3]);

				// Enforce user tag length limit.
				if (query_tag.length > 2 && query_tag != "rat") {
					query_tag = query_tag.substring(0, 2);
				}

				// If the colors are super borked, try to salvage them I guess.
				if(query_fg.split('.').length != 3) {
					query_fg = "255.255.255";
				}

				if(query_bg.split('.').length != 3) {
					query_bg = "0.0.0";
				}

				// Someone probably took the time to actually send the data correctly formatted 
				// in our strange and arbitrary format. At least we can prevent it breaking the UI sorta...
				if(query_score < 0) {
					query_score = 0;
				} else if(query_score > 999999) {
					query_score = 0;
				}

				// Ok, we're here, done what checking we can reasonably do. Lets call it good!
				let toAdd = {tag:query_tag, fg:query_fg, bg:query_bg, score:query_score};
				highscores.push(toAdd);
				res.send(formatSuccess(`Added item with data ${JSON.stringify(toAdd)}`));
			}
		}
	} catch {
		res.send(formatError('Received add, but there was an error processing the request.'));
	}
});

// Removes a specific index (or tries to, at least)
app.all(api_root + "remove", (req, res) => {
	try {
		let index = req.param("index");
		if (index === undefined) {
			res.send(formatError("Received remove, but didn't have expecetd param(s)."));
		} else {
			if(index == '' || index < 0 || index >= highscores.length) {
				res.send(formatError("Received remove, but value(s) were out of expected ranges."));
			} else {
				let item = highscores.splice(index, 1);
				res.send(formatSuccess(`Removed at index ${index} with data ${JSON.stringify(item)}`));
			}
		}
	} catch {
		res.send(formatError("Received remove, but there was an error processing the request."));
	}
});

// Returns all highscore data
app.all(api_root + "dump", (req, res) => {
	res.send({entries:highscores});
});



  /////////////////////////////
 // Actually run the server //
/////////////////////////////

// Bind to port and begin listening
app.listen(api_port, () => {
	GlobalLog.log(`Rat active, listening on port ${api_port}`)
});
