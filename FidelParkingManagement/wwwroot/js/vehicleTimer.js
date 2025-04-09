
    let timerInterval;

    function startTimer() {
        // Clear any existing timer
        clearInterval(timerInterval);

    // Get values
    const dateVal = document.getElementById("startDate").value;
    const timeVal = document.getElementById("startTime").value;

    if (!dateVal || !timeVal) {
        alert("Please enter both date and time.");
    return;
        }

    // Combine to one Date object
    const startTime = new Date(`${dateVal}T${timeVal}`);

        // Start interval
        timerInterval = setInterval(() => updateTimer(startTime), 1000);
    updateTimer(startTime); // run immediately
    }

    function updateTimer(startTime) {
        const now = new Date();
    let diff = Math.floor((now - startTime) / 1000); // seconds elapsed

    if (diff < 0) {
        document.getElementById("elapsedTimer").innerText = "Start time is in the future.";
    return;
        }

    const hours = Math.floor(diff / 3600);
    diff %= 3600;
    const minutes = Math.floor(diff / 60);
    const seconds = diff % 60;

    document.getElementById("elapsedTimer").innerText =
    `${pad(hours)}h : ${pad(minutes)}m : ${pad(seconds)}s`;
    }

    function pad(num) {
        return num.toString().padStart(2, '0');
    }

