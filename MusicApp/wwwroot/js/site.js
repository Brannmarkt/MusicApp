// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function stopOtherAudios(clickedAudio) {
    var audioElements = document.querySelectorAll("audio");
    audioElements.forEach(function (audio) {
        if (audio !== clickedAudio) {
            audio.pause();
            audio.currentTime = 0;
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    var audioElements = document.querySelectorAll("audio");
    audioElements.forEach(function (audio) {
        audio.addEventListener("play", function () {
            stopOtherAudios(audio);
        });
    });
});

//<script>
//    // Function to stop all audio elements except the one that is clicked
//    function stopOtherAudios(clickedAudio) {
//        var audioElements = document.querySelectorAll("audio");
//    audioElements.forEach(function (audio) {
//            if (audio !== clickedAudio) {
//        audio.pause();
//    audio.currentTime = 0;
//            }
//        });
//    }

//    // Add a click event listener to all audio elements
//    var audioElements = document.querySelectorAll("audio");
//    audioElements.forEach(function (audio) {
//        audio.addEventListener("play", function () {
//            stopOtherAudios(audio);
//        });
//    });
//</script>