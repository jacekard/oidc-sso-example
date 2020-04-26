function chooseRandomPageImage() {
    var pageImage = document.getElementById("pageImage");
    var images = ["krowa.png", "fatalnie.png", "fatcat.png", "weirdcat.png"];
    var rnd = Math.floor(Math.random() * images.length);
    var path = "img/";
    pageImage.src = path + images[rnd];
}

(function () {
    "use strict";

    chooseRandomPageImage();
})();