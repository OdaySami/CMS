importScripts('https://www.gstatic.com/firebasejs/9.1.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/9.1.1/firebase-messaging.js');

var config = {
    apiKey: "AIzaSyBfkTfF2_DKixbHwEjwBPxNetfGatbHN5c",
    authDomain: "cmcweb-132b7.firebaseapp.com",
    projectId: "cmcweb-132b7",
    storageBucket: "cmcweb-132b7.appspot.com",
    messagingSenderId: "310648206706",
    appId: "1:310648206706:web:1817fbe7c890de804619eb",
    measurementId: "G-3167JTH6W5"
};

firebase.initializeApp(config);

const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function(payload) {
    //// Customize notification here
    var notificationTitle = 'My Titile';
    var notificationOptions = {
        body: payload.data.body
    };

    return self.registration.showNotification(notificationTitle,
        notificationOptions);
});