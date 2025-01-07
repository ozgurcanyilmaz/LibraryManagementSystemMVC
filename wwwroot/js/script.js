function addBook(event) {
    event.preventDefault();
    var isbn = document.getElementById("book-isbn").value;
    var title = document.getElementById("book-title").value;
    var author = document.getElementById("book-author").value;
    var description = document.getElementById("book-description").value;
    var image = document.getElementById("book-image").files[0];

    console.log("Book added:");
    console.log("ISBN: " + isbn);
    console.log("Title: " + title);
    console.log("Author: " + author);
    console.log("Description: " + description);
    console.log("Image: " + image.name);

    // Clear input fields after adding
    document.getElementById("book-isbn").value = "";
    document.getElementById("book-title").value = "";
    document.getElementById("book-author").value = "";
    document.getElementById("book-description").value = "";
    document.getElementById("book-image").value = "";

    alert("Book added successfully!");
}

function updateBook(event) {
    event.preventDefault();

    var isbn = document.getElementById("book-isbn").value;
    var title = document.getElementById("book-title").value;
    var author = document.getElementById("book-author").value;
    var description = document.getElementById("book-description").value;
    var image = document.getElementById("book-image").files[0];

    if (!isbn) {
        console.error("Error: ISBN is required to update the book.");
        alert("Please provide the ISBN of the book you want to update.");
        return;
    }

    console.log("Book updated:");
    console.log("ISBN: " + isbn);
    console.log("Title: " + (title || "No changes"));
    console.log("Author: " + (author || "No changes"));
    console.log("Description: " + (description || "No changes"));
    console.log("Image: " + (image ? image.name : "No changes"));

    // Clear input fields after updating
    document.getElementById("book-isbn").value = "";
    document.getElementById("book-title").value = "";
    document.getElementById("book-author").value = "";
    document.getElementById("book-description").value = "";
    document.getElementById("book-image").value = "";

    alert("Book updated successfully!");
}

function logout() {
    window.location.href = "/";
}
