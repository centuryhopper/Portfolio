function initTinyMCERichText(editorId, dotNetReference) {
    // https://www.tiny.cloud/docs/tinymce/latest/user-formatting-options/#font_size_input_default_unit
    tinymce.init({
        selector: `#${editorId}`,
        height: 500,
        plugins: 'table wordcount image media code',
        toolbar: 'image | undo redo | media | code',
        image_file_types: 'jpg,.png',
        content_style: '.left { text-align: left; } ' +
          'img.left, audio.left, video.left { float: left; } ' +
          'table.left { margin-left: 0px; margin-right: auto; } ' +
          '.right { text-align: right; } ' +
          'img.right, audio.right, video.right { float: right; } ' +
          'table.right { margin-left: auto; margin-right: 0px; } ' +
          '.center { text-align: center; } ' +
          'img.center, audio.center, video.center { display: block; margin: 0 auto; } ' +
          'table.center { margin: 0 auto; } ' +
          '.full { text-align: justify; } ' +
          'img.full, audio.full, video.full { display: block; margin: 0 auto; } ' +
          'table.full { margin: 0 auto; } ' +
          '.bold { font-weight: bold; } ' +
          '.italic { font-style: italic; } ' +
          '.underline { text-decoration: underline; } ' +
          '.example1 {} ' +
          'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }' +
          '.tablerow1 { background-color: #D3D3D3; }',
        formats: {
          alignleft: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img,audio,video', classes: 'left' },
          aligncenter: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img,audio,video', classes: 'center' },
          alignright: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img,audio,video', classes: 'right' },
          alignfull: { selector: 'p,h1,h2,h3,h4,h5,h6,td,th,div,ul,ol,li,table,img,audio,video', classes: 'full' },
          bold: { inline: 'span', classes: 'bold' },
          italic: { inline: 'span', classes: 'italic' },
          underline: { inline: 'span', classes: 'underline', exact: true },
          strikethrough: { inline: 'del' },
          customformat: { inline: 'span', styles: { color: '#00ff00', fontSize: '20px' }, attributes: { title: 'My custom format'} , classes: 'example1'}
        },
        style_formats: [
          { title: 'Custom format', format: 'customformat' },
          { title: 'Align left', format: 'alignleft' },
          { title: 'Align center', format: 'aligncenter' },
          { title: 'Align right', format: 'alignright' },
          { title: 'Align full', format: 'alignfull' },
          { title: 'Bold text', inline: 'strong' },
          { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
          { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
          { title: 'Badge', inline: 'span', styles: { display: 'inline-block', border: '1px solid #2276d2', 'border-radius': '5px', padding: '2px 5px', margin: '0 2px', color: '#2276d2' } },
          { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' },
          { title: 'Image formats' },
          { title: 'Image Left', selector: 'img', styles: { 'float': 'left', 'margin': '0 10px 0 10px' } },
          { title: 'Image Right', selector: 'img', styles: { 'float': 'right', 'margin': '0 0 10px 10px' } },
        ],
        license_key: 'gpl',
        setup: (editor) => {
            editor.on('init', () => {
                if (dotNetReference) {
                    // console.log(`Editor with ID '${editorId}' initialized.`);
                    dotNetReference.invokeMethodAsync("OnTinyMCEInitialized"); // Notify Blazor
                }
            });

            editor.on('keyup', (e) => {
                if (dotNetReference)
                {
                    dotNetReference.invokeMethodAsync("OnTinyMCEChanged"); // Notify Blazor
                }
            });
        },

    });
}
 
function getTinyMCEContent (editorId) {
    const editor = tinymce.get(editorId);
    return editor ? editor.getContent() : "";
}
 
function loadTinyMCEContent(editorId, content) {
    const editor = tinymce.get(editorId);
    if (editor) {
        //console.log('setting contents');
        editor.selection.setContent(content);
    } else {
        console.error(`Editor with ID '${editorId}' not found.`);
    }
}

function resetTinyMCEEditor(editorId) {
    const editor = tinymce.get(editorId);
    if (editor) {
        editor.setContent('');
    } else {
        console.error(`Editor with ID '${editorId}' not found.`);
    }
}