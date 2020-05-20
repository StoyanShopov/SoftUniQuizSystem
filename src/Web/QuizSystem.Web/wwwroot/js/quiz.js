var number = Number(1);

var maxPages = document.getElementById("TotalQuestions").value;

function move(n) {
    if ((number + n) <= 0 || (number + n) > maxPages) {
        return;
    }
    document.getElementById("question-" + number).style.display = "none";
    document.getElementById("question-" + (number + n)).style.display = "inline";
    number += n;
}

function show(id) {
    document.getElementById("question-" + number).style.display = "none";
    document.getElementById("question-" + id).style.display = "inline";
    number = id;

}

function getTimeRemaining(endtime) {
    var t = Date.parse(endtime) - Date.parse(new Date());
    var seconds = Math.floor(t / 1000) % 60;
    var minutes = Math.floor(t / 1000 / 60) % 60;
    return {
        'total': t,
        'minutes': minutes,
        'seconds': seconds
    };
}

function initializeClock(id, endtime) {
    var clock = document.getElementById(id);
    var minutesSpan = clock.querySelector('.minutes');
    var secondsSpan = clock.querySelector('.seconds');

    function updateClock() {
        var t = getTimeRemaining(endtime);

        minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
        secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

        if (t.total <= 0) {
            clearInterval(timeinterval);
        }
    }

    updateClock();
    var timeinterval = setInterval(updateClock, 1000);
}

var deadline = new Date(Date.parse(new Date()) + 15 * 24 * 60 * 60 * 1000);
initializeClock('clockdiv', deadline);