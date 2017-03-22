# Myo-Snake
Fourth Year Project For Gesture based UI Module 

###### Martin Coleman
###### Ross Byrne

## Introduction
This project is for our fourth year Gesture based UI Module.

This is a universal app for the windows store. It can run on many platforms such as Windows 10 and Windows Phone. Find out more about universal apps here: https://msdn.microsoft.com/en-us/windows/uwp/get-started/whats-a-uwp

We chose this project as we wanted to work with Thalmic labs Myo Armband. The Myo Armband reads the electrical activity of your muscles to control technology with gestures and motion. 

Find out more about the armband here: https://www.myo.com/.

We had previously used a single myo in our labs and wanted to get two myo communicating together. We decided to create a multiplayer version of the classic game Snake.

(Details about game here)

## How To Play:

(Explain here)

## Technologies Used:

### Localisation:
The project has deStringifyed to the include Language Support for English (en-GB, en-US) and Irish (ga).
All text is stored in .resw files for each language.
Additional language support can be easily and quickly added to the App.

### MVVM
Model-view View-model
MVVM separates the responsibility for the appearance and layout of the UI from the responsibility for the presentation logic.

#### Model
The model represents the data we are dealing with.

#### View 
The view is what the end user sees and interacts with.

#### Viewmodel 
The Viewmodel connects the Model to the View.
It allows for loose coupling and binding of data.

### Local Storage
This project uses local storage to save highscores in a Json Array.

Initialise and get a handle on File
```
StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
StorageFile file;
```

Open File or create if doesnt exist
```
file = await storageFolder.CreateFileAsync("HighScores.txt", CreationCollisionOption.OpenIfExists);
string Json = await Windows.Storage.FileIO.ReadTextAsync(file);
```

Example of Json Stored Object
```json
{
  "Name": "Joe Bloggs",
  "HighScore": "2000"
}
```

## Json.Net 
This project uses Json.Net by newtonsoft as it handles serialisation and deserialisation very well.
<br>
http://www.newtonsoft.com/json

##Project Management
GitHub was used for managing the project's source control and issue tracking.

## Cloud
### Server
The Server we chose to save the highscores is NodeJS.
We chose NodeJS as its capable of Asynchronous I/O, its fast, lightweight, requires little setup, reliable and its use of javascript which allows handling and manipulation of the data such as determining rank of scores.

### Database
The database we chose is MongoDB. We chose mongoDB as it combines well with NodeJS with the use of the Mongoose connector.

Example of Javascript code to connect to mongoDB and save document
```
var mongoose = require('mongoose');
mongoose.connect('mongodb://localhost/test');

var Cat = mongoose.model('Cat', { name: String });

var kitty = new Cat({ name: 'Zildjian' });
kitty.save(function (err) {
  if (err) {
    console.log(err);
  } else {
    console.log('meow');
  }
});
```

Example of Json Stored Object in MongoDB
```json
{
  "Name": "Joe Bloggs",
  "HighScore": "2000"
}
```
