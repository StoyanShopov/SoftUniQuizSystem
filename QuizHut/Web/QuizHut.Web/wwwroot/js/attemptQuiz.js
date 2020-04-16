﻿$(document).ready(function () {
    var startBtn = document.getElementById('start');

    var questionsCount = 0;
    var counter = 1;

    if (startBtn) {
        var minsInput = document.getElementById("minutes");
        var mins = null;

        if (minsInput) {

            mins = minsInput.value;
        }
        var forms = document.getElementsByTagName('form');
        var form;
        if (forms.length > 1) {
            form = forms[1]
        } else {
            form = forms[0]
        }
        questionsCount = parseInt(form.id);
        var nextBtns = Array.from(document.getElementsByTagName('a')).filter(x => x.id.includes('next'));
        var prevBtns = Array.from(document.getElementsByTagName('a')).filter(x => x.id.includes('prev'));
        $(nextBtns).click(loadNextQuestion);
        $(prevBtns).click(loadPreviousQuestion);
        startQuizEventHandler(mins)
    }

    function startQuizEventHandler(mins) {
        $(startBtn).click(function () {
            if (mins) {
                $('#clockdiv').show();
                startTimer();
            }
            $('#pagging').show();
            $('#submit').show();
            $('#details').hide();
            showQuestion(counter);
        })
    }

    function loadNextQuestion(e) {
        e.preventDefault();
        hideQuestion(counter)
        if (counter == questionsCount) {
            showQuestion(counter);
        } else {
            showQuestion(counter + 1)
        }

        if (counter < questionsCount) {
            counter++;
        }
    }

    function loadPreviousQuestion(e) {
        e.preventDefault();
        hideQuestion(counter);
        if (counter == 1) {
            showQuestion(counter);
        } else {
            showQuestion(counter - 1)
        }

        if (counter > 1) {
            counter--;
        }
    }

    function showQuestion(counter) {
        $(`#${counter}`).show();
    }

    function hideQuestion(counter) {
        $(`#${counter}`).hide();
    }

    function startTimer() {
        let now = new Date($.now());
        let endTime = getEndDate(now, mins);
        initializeClock('clockdiv', endTime);

        function getTimeRemaining(endtime) {
            var t = Date.parse(endtime) - Date.parse(new Date());
            var seconds = Math.floor((t / 1000) % 60);
            var minutes = Math.floor((t / 1000 / 60) % 60);

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
                    $('#submit').click();
                }
            }

            updateClock();
            var timeinterval = setInterval(updateClock, 1000);
        }

        function getEndDate(dt, minutes) {
            return new Date(dt.getTime() + minutes * 60000).toString();
        }
    }
})