// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var multipleCardCarousel = document.querySelector(
    "#carouselIndexGameControls"
);
if (window.matchMedia("(min-width: 768px)").matches) {
    var carousel = new bootstrap.Carousel(multipleCardCarousel, {
        interval: false,
    });
    var carouselWidth = $(".carousel-inner")[0].scrollWidth;
    var cardWidth = $(".carousel-item").width();
    var scrollPosition = 0;
    $("#carouselIndexGameControls .carousel-control-next").on("click", function () {
        if (scrollPosition < carouselWidth - cardWidth * 4) {
            scrollPosition += cardWidth;
            $("#carouselIndexGameControls .carousel-inner").animate(
                { scrollLeft: scrollPosition },
                600
            );
        }
    });
    $("#carouselIndexGameControls .carousel-control-prev").on("click", function () {
        if (scrollPosition > 0) {
            scrollPosition -= cardWidth;
            $("#carouselIndexGameControls .carousel-inner").animate(
                { scrollLeft: scrollPosition },
                600
            );
        }
    });
} else {
    $(multipleCardCarousel).addClass("slide");
}

