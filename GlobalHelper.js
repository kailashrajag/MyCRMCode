// JavaScript source code
var Helper = window.Helper || {};
(
    function () {
        this.DoSomething = function (executioncontext) {
            alert("DoSomething message");


        }

    }
).call(Helper);