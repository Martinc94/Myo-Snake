# Myo-Snake
Fourth Year Project For Gesture based UI Module 

###### Martin Coleman
###### Ross Byrne

## Introduction
This project is for our fourth year Gesture based UI Module.

This is a universal app for the windows store. It can run on many platforms such as Windows 10 and Windows Phone. Find out more about universal apps here: https://msdn.microsoft.com/en-us/windows/uwp/get-started/whats-a-uwp

We chose this project as we wanted to work with Thalmic labs Myo Armband[1]. The Myo Armband reads the electrical activity of your muscles to control technology with gestures and motion. 

Find out more about the armband by clicking on link in references below[1].

We had previously used a single myo in our labs and wanted to get two myo communicating together. We decided to create a multiplayer version of the classic game Snake.

## Snake
Snake is the common name for a videogame concept where the player maneuvers a line which grows in length, with the line itself being a primary obstacle. The concept originated in the 1976 arcade game Blockade[3]. It regained popularity in 1998 when it was included on Nokia phones.

## Gameplay
Player controls a square as it moves foward around the screen. The tail of the snake trails the head. 
The player can move the direction the head is travelling in to the right or the left. 

The players aim is to collect pickups around the screen. The snake grows in length each time it gathers a pickup. If the snake eats its body its game over. As we have added a second snake if player hits another snake its game over for that player.

This application has both one and two player modes and snakes can be controlled by a Myo Armband and by Keyboard presses.

## Controls:
### Myo:
The wave in gesture moves the head of the snake to the left.

The wave out gesture moves the head of the snake to the right.

### Keyboard:
Left key press moves the head of the snake to the left for player one.

Right key press moves the head of the snake to the right for player one.

A key press moves the head of the snake to the left for player two.

D key press moves the head of the snake to the right for player two.

## Technologies Used:

### MyoSharp
MyoSharp is a C# Wrapper for the Myo Armband. It provides an interface for working with Myo Armband[2].

Example of C# code using MyoSharp wrapper
```
private static void Main()
        {
            // create a hub that will manage Myo devices for us
            using (var channel = Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()))))
            using (var hub = Hub.Create(channel))
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.OrientationDataAcquired -= Myo_OrientationDataAcquired;
                };

                // start listening for Myo data
                channel.StartListening();

                // wait on user input
                ConsoleHelper.UserInputLoop(hub);
            }

```

### Localisation:
The project has been deStringifyed to the include Language Support for English (en-GB, en-US) and Irish (ga).
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

## Json.Net 
This project uses Json.Net by newtonsoft as it handles serialisation and deserialisation very well.

##Project Management
GitHub was used for managing the project's source control and issue tracking.

## Cloud
### Server
The Server we chose to save the highscores is NodeJS.
We chose NodeJS as its capable of Asynchronous I/O, its fast, lightweight, requires little setup, reliable and its use of javascript which allows handling and manipulation of the data such as determining rank of scores.

#### NodeJS
Node is an asynchronous event driven JavaScript runtime that is designed to build scalable network applications. Node is lightweight yet extremely effective and handling HTTP Services such as GET, POST, PUT and DELETE.

### Database
The database we chose is MongoDB. We chose mongoDB as it combines well with NodeJS with the use of the Mongoose connector. MongoDB stores documents in JSON format.

#### Mongoose
Mongoose is the driver that connects the Node server to the MongoDB database.

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
## Note
If running application in development mode you may need to install Microsoft Code Contracts on your machine.
Links is in references section[10].

## References
1. https://www.myo.com/
2. https://github.com/tayfuzun/MyoSharp
3. https://en.wikipedia.org/wiki/Snake_(video_game)
4. http://www.newtonsoft.com/json
5. https://msdn.microsoft.com/en-us/windows/uwp/get-started/whats-a-uwp
6. https://nodejs.org/en/
7. https://www.mongodb.com/
8. http://mongoosejs.com/
9. https://msdn.microsoft.com/en-us/library/hh848246.aspx
10. https://marketplace.visualstudio.com/items?itemName=RiSEResearchinSoftwareEngineering.CodeContractsforNET
