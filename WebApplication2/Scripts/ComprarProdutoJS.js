let imgs = document.getElementById("imgs");

function updateSelected(e, img) {
    let swatches = e.parentElement.children;
    let selected = e.id;

    console.log(img);

    imgs.src = img;

    currentlySelectedColor = colorNameText;
    updateColorText();
    for (var i = 0; i < swatches.length; i++) {
        if (swatches[i].id == selected) {
            swatches[i].className = "swatch selected";
        } else {
            swatches[i].className = "swatch";
        }
    }
    showButton();
}

function showButton() {
    button.style.opacity = "1";
    button.style.height = "45px";
}