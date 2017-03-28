//Api server for Getting and posting Highscores for Uwp App
//Author Martin Coleman

//System Variables
var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var morgan = require('morgan');
var mongoose = require('mongoose');
var config = require('./config/database');
var port = process.env.PORT || 3000;

//Schema for loading and storing data model
var HighScore = require('./app/models/HighScore');
////////////////////////////////////////////////////////////////////////////////////////////////////////

// log to console
app.use(morgan('dev'));

// parse application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({
    extended: false
}))

// parse application/json
app.use(bodyParser.json())

// connect to Mongodb database using mongoose
var db = mongoose.connect(config.database);

// bundle our routes
var apiRoutes = express.Router();

////////////////////////////////////////////////////////////////////////////////////
//ROUTES///////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////

//Post for setting up scores // run at setup to create db
apiRoutes.post('/setupScores', function (req, res) {
    var hScore = new HighScore();

    if (req.body.password == "passwordForDB") {
        //save
        hScore.save(function (err) {
            if (err)
                res.send(err);
            res.json({
                success: true,
                msg: 'Scores Setup'
            });
        });
    } //end if 
    else
        res.send("Scores not setup wrong password");
});
//end setupScores/////////////////////////////////////////////////////////////////////

apiRoutes.post('/postScore', function (req, res) {
    //get high scores
    HighScore.findOne({}, function (err, hScore) {
        if (err) throw err;
        if (!hScore) {
            return res.status(403).send({
                success: false,
                msg: 'Scores not found.'
            });
        } else {
            //if valid HighScore
            if (req.body.name && req.body.score) {
                var scoreArray = [];
                var temp = {}
                var name = {};
                var score = {};
                var i;

                //array of score name pairs
                for (i = 1; i < 25; i += 1) {
                    temp = {};
                    temp.name = hScore['name' + i];
                    temp.score = hScore['score' + i];

                    if (temp.name || temp.score) {
                        scoreArray.push(temp);
                    }

                } //end for

                //add new score to array
                temp = {};
                temp.name = req.body.name;
                temp.score = req.body.score;

                scoreArray.push(temp);

                //sort array by score
                scoreArray.sort(function (a, b) {
                    return parseInt(b.score) - parseInt(a.score);
                });

                //save top 25
                for (i = 0; i < 25; i += 1) {
                    //if vaild score save
                    if (scoreArray[i]) {

                        hScore['name' + (i + 1)] = scoreArray[i].name;

                        hScore['score' + (i + 1)] = scoreArray[i].score;

                        //console.log(hScore['name' + (i + 1)]);
                        //console.log(hScore['score' + (i + 1)]);
                    }
                } //end for

                //save to db
                hScore.save(function (err) {
                    if (err)
                        res.send(err);
                    res.json({
                        success: true,
                        msg: 'Score Saved'
                    });
                });

            } //end if

        } //end else
    });

});

//Get high scores
apiRoutes.get('/getScores', function (req, res) {
    HighScore.findOne({}, function (err, hScore) {
        if (err) throw err;
        if (!hScore) {
            return res.status(403).send({
                success: false,
                msg: 'Scores not found.'
            });
        } else {
            //return scores
            var scoreArray = [];
            var temp = {}
            var name = {};
            var score = {};
            var i;

            //array of score name pairs
            for (i = 1; i < 26; i += 1) {

                temp = {};
                temp['name' + i] = hScore['name' + i];
                temp['score' + i] = hScore['score' + i];

                if (hScore['name' + i]) {
                    scoreArray.push(temp);
                }

            } //end for

            return res.json(scoreArray);

        } //end else
    }) //end find one
});
//end getScores////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////
//Init + Start Server
////////////////////////////////////////////////////////////////////////////////////////////////

// connect the api routes under /api/*
app.use('/api', apiRoutes);

// Start the server
app.listen(port);
console.log('Server live on: http://localhost:' + port);