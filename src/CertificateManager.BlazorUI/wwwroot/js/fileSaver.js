window.saveAsFile = function (fileName, byteBase64) {
    const linkSource = `data:application/pdf;base64,${byteBase64}`;
    const downloadLink = document.createElement("a");
    const blob = new Blob([byteBase64], { type: "application/pdf" });

    const url = URL.createObjectURL(blob);

    downloadLink.href = url;
    downloadLink.download = fileName;
    downloadLink.click();

    URL.revokeObjectURL(url);
};
