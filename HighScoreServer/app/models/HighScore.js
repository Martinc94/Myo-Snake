var mongoose     = require('mongoose');
var Schema       = mongoose.Schema;

var scoreSchema   = new Schema({
    name1: String,
    score1: String,
    name2: String,
    score2: String, 
    name3: String,
    score3: String, 
    name4: String,
    score4: String, 
    name5: String,
    score5: String, 
    name6: String,
    score6: String, 
    name7: String,
    score7: String, 
    name8: String,
    score8: String, 
    name9: String,
    score9: String, 
    name10: String,
    score10: String,
    name11: String,
    score11: String,
    name12: String,
    score12: String, 
    name13: String,
    score13: String, 
    name14: String,
    score14: String, 
    name15: String,
    score15: String, 
    name16: String,
    score16: String, 
    name17: String,
    score17: String, 
    name18: String,
    score18: String, 
    name19: String,
    score19: String, 
    name20: String,
    score20: String,
    name21: String,
    score21: String,
    name22: String,
    score22: String,
    name23: String,
    score23: String,
    name24: String,
    score24: String,
    name25: String,
    score25: String,
});

module.exports = mongoose.model('HighScore', scoreSchema);