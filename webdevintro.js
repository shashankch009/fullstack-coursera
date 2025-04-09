
document.addEventListener('DOMContentLoaded', () => { 
    
    const button = document.getElementById('fetch-button');
    button.addEventListener('click', handleButtonClick);
});

async function handleButtonClick() {

    const url = 'https://jsonplaceholder.typicode.com/users';
    let paraContent = "";
    try {
        const response = await fetch(url, {method : 'GET'});
        if(!response.ok) {
            paraContent = "HTTP error! Status: " + response.status;
        }
        else {
            const data = await response.json();
            paraContent = JSON.stringify(data, null, 2);
        }
    }
    catch (error) {
        console.error('Error fetching users:', error);
        paraContent = 'Error fetching users.' + error;
    }
    document.getElementById('response-para').textContent = paraContent;
    console.log(paraContent);
}
