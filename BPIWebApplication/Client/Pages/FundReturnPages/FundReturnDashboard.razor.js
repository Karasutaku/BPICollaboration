export function showAlert(message) {
    alert(message);
}

export async function previewFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();

    const blob = new Blob([arrayBuffer], { type: 'application/pdf' });
    const url = URL.createObjectURL(blob);
    window.open(url);

    //const anchorElement = document.createElement('a');
    //anchorElement.href = url;
    //anchorElement.download = fileName ?? '';
    //anchorElement.click();
    //anchorElement.remove();
}