function validateForm(event) {
    event.preventDefault(); // Prevent form submission
    const password1 = document.getElementById('password1').value;
    const password2 = document.getElementById('password2').value;

    if (password1 !== password2) {
        alert('Passwords do not match!');
        return false;
    }

    alert('Form submitted successfully!');
    document.getElementById('registrationForm').submit(); // Submit the form if validation passes
}