function goBack() {
    // Index sayfasına yönlendir
    window.location.href = "/";
}

function librarianLogin(event) {
    event.preventDefault();
    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();

    // Temp login bilgileri
    const librarianUsername = "admin";
    const librarianPassword = "admin";

    if (username === librarianUsername && password === librarianPassword) {
        alert("Welcome, Librarian!");
        // Admin Portal URL'sine yönlendir
        window.location.href = "/Admin/Portal";
    } else {
        alert("Invalid username or password. Please try again.");
    }
}
