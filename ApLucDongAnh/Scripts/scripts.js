AOS.init({
    once: true,
});

function homeJs() {
    $(".project-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        arrows: false,
        responsive: [
            {
                breakpoint: 900,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });

    $(".business-connect-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 5,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 900,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });

    $(".category-hot-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: false,
        speed: 1000,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });

    $('.service-list').slick({
        infinite: true,
        slidesToShow: 4,
        slidesToScroll: 1,
        autoplay: false,
        speed: 1000,
        autoplaySpeed: 3000,
        dots: false,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 1400,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 900,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });

    $('.video-slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        fade: true,
        asNavFor: '.video-slider-nav',
        arrows: false,
    });
    $('.video-slider-nav').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.video-slider-for',
        dots: false,
        speed: 1000,
        centerMode: false,
        focusOnSelect: true,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1,
                }
            },
        ]
    });

    $(".feedback-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        arrows: false,
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });

    $(".article-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 2,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
}

function productDetail() {
    $('.slider-for').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        fade: true,
        asNavFor: '.slider-nav',
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
    });
    $('.slider-nav').slick({
        slidesToShow: 5,
        slidesToScroll: 1,
        asNavFor: '.slider-for',
        speed: 1000,
        dots: false,
        focusOnSelect: true,
        arrows: false,
        vertical: true,
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 5,
                    slidesToScroll: 1,
                    vertical: false,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1,
                    vertical: false,
                }
            },
        ]
    });

    $(".related-product-list").slick({
        dots: false,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
}

function serviceCategory() {
    $(".feedback-customer-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        arrows: false,
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
}

function introduce() {
    $(".content-intro-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        arrows: false,
    });

    $('.banner-introduce').slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: false,
        speed: 1000,
        autoplaySpeed: 3000,
        dots: false,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
    });

    $('.production-img-list').slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: false,
        speed: 1000,
        autoplaySpeed: 3000,
        dots: false,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
    });

    $('.history-list').slick({
        infinite: false,
        slidesToShow: 5,
        slidesToScroll: 1,
        autoplay: false,
        speed: 1000,
        autoplaySpeed: 3000,
        dots: false,
        variableWidth: true,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ],
    });

    $(".achievement-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        arrows: false,
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
}

function projectDetail() {
    $('.project-img-list').slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: false,
        speed: 1000,
        autoplaySpeed: 3000,
        dots: false,
        prevArrow: '<button class="chevron-prev"><i class="far fa-chevron-left"></i></button>',
        nextArrow: '<button class="chevron-next"><i class="far fa-chevron-right"></i></button>',
    });
}

function getProject(action) {
    $('#project-cat .nav-link').click(function () {
        let catId = parseInt($(this).val());

        $('body').append('<div class="loading"><i class="fad fa-spin fa-spinner"></i></div>');
        $.get(action, { catId: catId }, function (data) {
            $('#list-project-sort').empty();
            $('#list-project-sort').html(data);
        }).then(function () {
            $('.loading').remove();
        });
    })
}


$(document).ready(function () {
    $(".partner-list").slick({
        dots: true,
        infinite: true,
        slidesToShow: 6,
        slidesToScroll: 1,
        speed: 1000,
        autoplay: false,
        autoplaySpeed: 3000,
        arrows: false,
        responsive: [
            {
                breakpoint: 830,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            }
        ]
    });

    $('.number').countUp();

    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $(".header-mb").addClass("active");
        } else {
            $(".header-mb").removeClass("active");
        }
        if ($(this).scrollTop() > 200) {
            $(".header-sticky-home").addClass("active");
            $(".btn-scroll").fadeIn(200);
        } else {
            $(".header-sticky-home").removeClass("active");
            $(".btn-scroll").fadeOut(200);
        }
    });

    $(".btn-scroll").click(function (event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, 300);
    });

    $(".hamburger").click(function () {
        $(this).toggleClass("active");
        $(".menu-mb").toggleClass("active");
        $(".overlay").toggleClass("active");
    });

    $(".overlay").click(function () {
        $(this).removeClass("active");
        $(".menu-mb").removeClass("active");
        $(".hamburger").removeClass("active");
    });

    $(".btn-search").click(function () {
        $(".body-overlay").addClass('active');
        $(".site-search").addClass('active');
        function delay() {
            $(".site-search .form-control").focus();
        }

        setTimeout(delay, 300);
    });

    $(".body-overlay, .site-search-close").click(function () {
        $(".body-overlay").removeClass('active');
        $(".site-search").removeClass('active');
    });

    $(".toggle").click(function () {
        $(this).toggleClass('active');
        $(this).siblings('.sub-nav-mb').slideToggle();
    });

    $(".project-item").hover(
        function () {
            $(this).addClass('active');
            $(this).parents('.slick-slide').siblings().find('.project-item').removeClass('active');
        },
        function () {
            $(this).parents('.slick-slide').siblings().find('.project-item').removeClass('active');
        }
    );

    var decoration = $(".decoration");
    $(window).on('scroll', function () {
        decoration.each(function () {
            if ($(this).offset().top <= $(window).scrollTop() + $(window).height() * 0.75) {
                $(this).addClass('active');
            }
        });
    });

    $('.service-item-link').hover(
        function () {
            $(this).find('.excerpt').slideDown(300);
        },
        function () {
            $(this).find('.excerpt').slideUp(300);
        }
    );

    const loadG = sessionStorage.getItem("loadGT");
    if (loadG === "1") {
        $.getScript("/Scripts/Google_element.js");
    }
})

$(function () {
    $(".contact-form").on("submit", function (e) {
        e.preventDefault();
        $.post("/Home/ContactForm", $(this).serialize(), function (data) {
            if (data.status) {
                $.toast({
                    heading: "Gửi liên hệ thành công",
                    text: data.msg,
                    icon: "success",
                    position: "bottom-right"
                });
                $(".contact-form").trigger("reset");
            } else {
                $.toast({
                    heading: "Liên hệ không thành công",
                    text: data.msg,
                    icon: "error",
                    position: "bottom-right"
                });
            }
        });
    });

    $(".form-subcribe").on("submit", function (e) {
        e.preventDefault();
        $.post("/Home/SubcribeForm", $(this).serialize(), function (data) {
            if (data.status) {
                $.toast({
                    heading: "Liên hệ thành công",
                    text: data.msg,
                    icon: "success",
                    position: "bottom-right"
                });
                $(".form-subcribe").trigger("reset");
            } else {
                $.toast({
                    heading: "Liên hệ không thành công",
                    text: data.msg,
                    icon: "error",
                    position: "bottom-right"
                });
            }
        });
    });

    $("#orderForm").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/Home/OrderForm", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Đặt hàng thành công',
                        text: data.msg,
                        icon: 'success',
                        position: "bottom-right"
                    })
                    $("#orderForm").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Đặt hàng không thành công',
                        text: data.msg,
                        icon: 'error',
                        position: "bottom-right"
                    })
                }
            });
        }
    });
});

function Sort(action) {
    $("#sort").change(function () {
        let sort = $(this).val();
        let url = $("input[name=currentUrl]").val();
        let keywords = $("input[name=keywords][type=hidden]").val();
        let title = $('.breadcrumb-item.active').text();

        url = url.split('/').at(-1);

        window.history.pushState(title, '', url);

        $('body').append('<div class="loading"><i class="fad fa-spin fa-spinner"></i></div>');
        $.get(action, { keywords: keywords, url, sort: sort }, function (data) {
            $('#list-item-sort').empty();
            $('#list-item-sort').html(data);
        }).then(function () {
            $('.loading').remove();
        });
    });
}

function GTranslateFireEvent(a, b) {
    try {
        if (document.createEvent) {
            var c = document.createEvent("HTMLEvents");
            c.initEvent(b, true, true);
            a.dispatchEvent(c);
        } else {
            var c = document.createEventObject();
            a.fireEvent("on" + b, c);
        }
    } catch (e) {
    }
}
function doTranslate(a) {
    const loadG = sessionStorage.getItem("loadGT");
    if (loadG === null) {
        $.getScript("/Scripts/Google_element.js");
        sessionStorage.setItem("loadGT", 1);
    }

    if (a.value) a = a.value;
    if (a === "") return;
    const b = a.split("|")[1];
    var c;
    const d = document.getElementsByTagName("select");
    for (let i = 0; i < d.length; i++) if (d[i].className === "goog-te-combo") c = d[i];
    if (document.getElementById("google_translate_element2") === null || document.getElementById('google_translate_element2').innerHTML.length === 0 || c.length === 0 || c.innerHTML.length === 0) {
        setTimeout(function () {
            doTranslate(a);
        }, 500);
    } else {
        c.value = b;
        GTranslateFireEvent(c, "change");
        GTranslateFireEvent(c, "change");
    }
}
function googleTranslateElementInit() {
    const translateElement = new google.translate.TranslateElement({
        pageLanguage: "vi",
        includedLanguages: "vi,en,zh-CN,de,fr,ja,ko,ru",
        multilanguagePage: false,
        autoDisplay: false
    }, "google_translate_element2");
    $("#goog-gt-tt h1").remove();
}