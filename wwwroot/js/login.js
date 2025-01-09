async function librarianLogin(event) {
    event.preventDefault(); // Formun otomatik yenilenmesini engeller

    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();

    console.log("Login attempt:", username, password);

    try {
        // Backend'e giriş isteği gönder
        const response = await fetch('/api/authenticate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        });

        if (response.ok) {
            const data = await response.json();

            // Rolü kontrol et ve yönlendir
            if (data.role === 'Admin') {
                alert("Welcome, Librarian!");
                window.location.href = "/Admin/Portal";
            } else {
                alert("You do not have permission to access this page.");
            }
        } else {
            const error = await response.json();
            alert(error.message || "Invalid username or password.");
        }
    } catch (error) {
        console.error("Login error:", error);
        alert("An error occurred while logging in. Please try again.");
    }
}

async function loginn(event) {
    event.preventDefault(); // Formun otomatik yenilenmesini engeller

    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value.trim();

    try {
        // Backend'e giriş isteği gönder
        const response = await fetch('/api/authenticate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        });

        if (response.ok) {
            const data = await response.json();

            // Rolü kontrol et ve yönlendir
            if (data.role === 'Student') {
                alert("Welcome, Student!");
                window.location.href = "/Student/Page";
            } else {
                alert("You do not have permission to access this page.");
            }
        } else {
            const error = await response.json();
            alert(error.message || "Invalid username or password.");
        }
    } catch (error) {
        console.error("Login error:", error);
        alert("An error occurred while logging in. Please try again.");
    }
}

function goBack() {
    // Index sayfasına yönlendir
    window.location.href = "/";
}
