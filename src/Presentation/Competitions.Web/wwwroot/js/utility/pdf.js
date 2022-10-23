function PDF(id) {
    document.getElementById(id).setAttribute("style","padding : 15px 20px;");
    printJS({
        printable: id,
        type: 'html',
        css: ['../../css/main.min.css', 'https://pro.fontawesome.com/releases/v5.10.0/css/all.css'],
        targetStyles: ['*'],
        scanStyles: false
    })
    document.getElementById(id).setAttribute("style" , "");
}