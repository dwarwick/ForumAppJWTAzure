tinyMceConf =
{
    menubar: false,
    resize: true,
    statusbar: false,
    plugins: 'autoresize codesample link',
    link_target_list: false,
    link_assume_external_targets: 'https',
    link_context_toolbar: true,
    link_default_protocol: 'https',
    paste_data_images: false,    
    content_css: ['lib/tinymce/skins/content/tinymce-5-dark/content.min.css', "css/app-dark.css"],
    toolbar: 'undo redo | copy cut paste pastetext | codesample link openlink | bold italic underline subscript superscript strikethrough | alignleft aligncenter alignright alignjustify | blockquote'
}

editorConf =
{    
    menubar: false,
    toolbar: false,
    resize: true,
    statusbar: false,
    plugins: 'autoresize codesample',
    content_css: ['lib/tinymce/skins/content/tinymce-5-dark/content.min.css', 'css/app-dark.css'],
    autoresize_overflow_padding: 5,
    autoresize_bottom_margin: 5
};

