$(function () {
    var JustBlog = {};

    JustBlog.GridManager = {};

    //************************* POSTS GRID
    JustBlog.GridManager.postsGrid = function (gridName, pagerName) {
        //*** Event handlers
        var afterclickPgButtons = function (whichbutton, formid, rowid) {
            tinyMCE.get("ShortDescription").setContent(formid[0]["ShortDescription"].value);
            tinyMCE.get("Description").setContent(formid[0]["Description"].value);
        };

        var afterShowForm = function (form) {

            tinymce.init({
                 language:"el",
                selector: "textarea",
                theme: "modern",
                height: 300,
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor"
                ],
                toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media | forecolor backcolor emoticons",
               // connector: '@Model.baseUrl' + 'Admin/Upload',
                connector: "Upload/Index",


                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
            });





            //tinymce.init({
            //    language:"el",
            //     selector: "textarea",
            //    theme: "modern",
            //    height: 300,
            //    plugins: [
            //        "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
            //        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
            //        "save table contextmenu directionality emoticons template paste textcolor"
            //    ],
            //    content_css: "css/metro-bootstrap.min.css",
            //    toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",
            //    file_browser_callback: function(field, url, type, win) {
            //        tinyMCE.activeEditor.windowManager.open({
            //            url: '/FileBrowser/FileBrowser.aspx?caller=tinymce4&langCode=en&type=' + type,
            //            title: 'File Browser',
            //            width: 700,
            //            height: 500,
            //            inline: true,
            //            close_previous: false
            //        }, {
            //            window: win,
            //            field: field
            //        });
            //        return false;
            //    }

            //});


         // tinyMCE.execCommand('mceAddControl', false, "ShortDescription");
        // tinyMCE.execCommand('mceAddControl', false, "Description");
        };

        var onClose = function (form) {
            //tinymce4 is removed like this
            tinymce.remove('textarea');
           //tinyMCE.execCommand('mceRemoveControl', false, "ShortDescription");
           //tinyMCE.execCommand('mceRemoveControl', false, "Description");
            
        };

        var beforeSubmitHandler = function (postdata, form) {
            var selRowData = $(gridName).getRowData($(gridName).getGridParam('selrow'));
            if (selRowData["PostedOn"])
                postdata.PostedOn = selRowData["PostedOn"];
            postdata.ShortDescription = tinyMCE.get("ShortDescription").getContent();
            postdata.Description = tinyMCE.get("Description").getContent();

            return [true];
        };

        var colNames = [
                'Id',
                'Title',
                'Short Description',
                'Description',
                'Category',
                'Category',
                'Tags',
                'Meta',
                'Url Slug',
                'Published',
                'Posted On',
                'Modified'
                 //'Image'
        ];

        var columns = [];

        columns.push({
            name: 'Id',
            hidden: true,
            key: true
        });

        columns.push({
            name: 'Title',
            index: 'Title',
            width: 250,
            editable: true,
            editoptions: {
                size: 43,
                maxlength: 500
            },
            editrules: {
                required: true
            },
            formatter: 'showlink',
            formatoptions: {
                target: "_new",
                baseLinkUrl: '/Admin/GoToPost'
            }
        });

        columns.push({
            name: 'ShortDescription',
            index: 'ShortDescription',
            width: 250,
            editable: true,
            sortable: false,
            hidden: true,
            edittype: 'textarea',
            editoptions: {
                rows: "10",
                cols: "100"
            },
            editrules: {
                custom: true,
                custom_func: function (val, colname) {
                    val = tinyMCE.get("ShortDescription").getContent();
                    if (val) return [true, ""];
                    return [false, colname + ": Field is required"];
                },
                edithidden: true
            }
        });

        columns.push({
            name: 'Description',
            index: 'Description',
            width: 250,
            editable: true,
            sortable: false,
            hidden: true,
            edittype: 'textarea',
            editoptions: {
                rows: "40",
                cols: "100"
            },
            editrules: {
                custom: true,
                custom_func: function (val, colname) {
                    val = tinyMCE.get("Description").getContent();
                    if (val) return [true, ""];
                    return [false, colname + ": Field is requred"];
                },
                edithidden: true
            }
        });

        columns.push({
            name: 'Category.Id',
            hidden: true,
            editable: true,
            edittype: 'select',
            editoptions: {
                style: 'width:250px;',
                dataUrl: '/Admin/GetCategoriesHtml'
            },
            editrules: {
                required: true,
                edithidden: true
            }
        });

        columns.push({
            name: 'Category.Name',
            index: 'Category',
            width: 150
        });

        columns.push({
            name: 'Tags',
            width: 150,
            editable: true,
            edittype: 'select',
            editoptions: {
                style: 'width:250px;',
                dataUrl: '/Admin/GetTagsHtml',
                multiple: true
            },
            editrules: {
                required: true
            }
        });

        //columns.push({
        //    name: 'Meta',
        //    width: 250,
        //    sortable: false,
        //    editable: true,
        //    edittype: 'textarea',
        //    editoptions: {
        //        rows: "2",
        //        cols: "40",
        //        maxlength: 1000
        //    },
        //    editrules: {
        //        required: true
        //    }
        //});

        columns.push({
            name: 'Meta',
            width: 250,
            sortable: false,
            editable: true,
           
            editoptions: {
                 size: 43,
                maxlength: 200
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'UrlSlug',
            width: 200,
            sortable: false,
            editable: true,
            editoptions: {
                size: 43,
                maxlength: 200
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'Published',
            index: 'Published',
            width: 100,
            align: 'center',
            editable: true,
            edittype: 'checkbox',
            editoptions: {
                value: "true:false",
                defaultValue: 'false'
            }
        });

        columns.push({
            name: 'PostedOn',
            index: 'PostedOn',
            width: 150,
            align: 'center',
            sorttype: 'date',
            datefmt: 'm/d/Y'
        });

        columns.push({
            name: 'Modified',
            index: 'Modified',
            width: 100,
            align: 'center',
            sorttype: 'date',
            datefmt: 'm/d/Y'
        });

        ////////////////adding image

        //columns.push({
        //    name: 'Image',
        //    index: 'Image',
        //    align: 'left',
        //    editable: true,
        //    edittype: 'file',
        //    editoptions: {
        //        enctype: "multipart/form-data"
        //    },
        //    width: 210,
        //    align: 'center',
        //    //formatter: jgImageFormatter,
        //    //afterSubmit: UploadImage,
        //    search: false
        //});

        ////////////////adding image

        $(gridName).jqGrid({
            url: '/Admin/Posts',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',
            toppager: true,

            colNames: colNames,
            colModel: columns,

            pager: pagerName,
            rownumbers: true,
            rownumWidth: 40,
            rowNum: 10,
            rowList: [10, 20, 30],

            sortname: 'PostedOn',
            sortorder: 'desc',
            viewrecords: true,

            jsonReader: {
                repeatitems: false
            },

            afterInsertRow: function (rowid, rowdata, rowelem) {
                var published = rowdata["Published"];

                if (!published) {
                    $(gridName).setRowData(rowid, [], {
                        color: '#9D9687'
                    });
                    $(gridName + " tr#" + rowid + " a").css({
                        color: '#9D9687'
                    });
                }

                var tags = rowdata["Tags"];
                var tagStr = "";

                $.each(tags, function (i, t) {
                    if (tagStr) tagStr += ", "
                    tagStr += t.Name;
                });

                $(gridName).setRowData(rowid, { "Tags": tagStr });
            }
        });

        // configuring add options
        var addOptions = {
            url: '/Admin/AddPost',
            addCaption: 'Add Post',
            processData: "Saving...",
            width: 900,
            closeAfterAdd: true,
            closeOnEscape: true,
            afterclickPgButtons: afterclickPgButtons,
            afterShowForm: afterShowForm,
            onClose: onClose,
            afterSubmit: JustBlog.GridManager.afterSubmitHandler,
            beforeSubmit: beforeSubmitHandler
        };

        var editOptions = {
            url: '/Admin/EditPost',
            editCaption: 'Edit Post',
            processData: "Saving...",
            width: 900,
            closeAfterEdit: true,
            closeOnEscape: true,
            afterclickPgButtons: afterclickPgButtons,
            afterShowForm: afterShowForm,
            onClose: onClose,
            afterSubmit: JustBlog.GridManager.afterSubmitHandler,
            beforeSubmit: beforeSubmitHandler
        };

        var deleteOptions = {
            url: '/Admin/DeletePost',
            caption: 'Delete Post',
            processData: "Saving...",
            msg: "Delete the Post?",
            closeOnEscape: true,
            afterSubmit: JustBlog.GridManager.afterSubmitHandler
        };

        $(gridName).navGrid(pagerName, { cloneToTop: true, search: false }, editOptions, addOptions, deleteOptions);
    };

    //uploading image

    //function ajaxFileUpload(id) {
    //    $.ajaxFileUpload
    // (
    //     {
    //         url: '/Admin/UploadImage',
    //         secureuri: false,
    //         fileElementId: 'Image',
    //         dataType: 'json',
    //         data: { id: id },
    //         success: function (data, status) {

    //             if (typeof (data.isUploaded) != 'undefined') {
    //                 if (data.isUploaded == true) {
    //                     return;
    //                 } else {
    //                     alert(data.message);
    //                 }
    //             }
    //             else {
    //                 return alert('Failed to upload image!');
    //             }
    //         },
    //         error: function (data, status, e) {
    //             return alert('Failed to upload image!');
    //         }
    //     }
    // )

    //    return false;
    //}

    /////////////////// apo edw mexri katw den yparxei

    //************************* CATEGORIES GRID
    JustBlog.GridManager.categoriesGrid = function (gridName, pagerName) {
        var colNames = ['Id', 'Name', 'Url Slug', 'Description'];

        var columns = [];

        columns.push({
            name: 'Id',
            index: 'Id',
            hidden: true,
            sorttype: 'int',
            key: true,
            editable: false,
            editoptions: {
                readonly: true
            }
        });

        columns.push({
            name: 'Name',
            index: 'Name',
            width: 200,
            editable: true,
            edittype: 'text',
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'UrlSlug',
            index: 'UrlSlug',
            width: 200,
            editable: true,
            edittype: 'text',
            sortable: false,
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'Description',
            index: 'Description',
            width: 200,
            editable: true,
            edittype: 'textarea',
            sortable: false,
            editoptions: {
                rows: "4",
                cols: "28"
            }
        });

        $(gridName).jqGrid({
            url: '/Admin/Categories',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',
            toppager: true,
            colNames: colNames,
            colModel: columns,
            pager: pagerName,
            rownumbers: true,
            rownumWidth: 40,
            rowNum: 500,
            sortname: 'Name',
            loadonce: true,
            jsonReader: {
                repeatitems: false
            }
        });

        var editOptions = {
            url: '/Admin/EditCategory',
            width: 400,
            editCaption: 'Edit Category',
            processData: "Saving...",
            closeAfterEdit: true,
            closeOnEscape: true,
            afterSubmit: function (response, postdata) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Failed to get result from server.", null];
            }
        };

        var addOptions = {
            url: '/Admin/AddCategory',
            width: 400,
            addCaption: 'Add Category',
            processData: "Saving...",
            closeAfterAdd: true,
            closeOnEscape: true,
            afterSubmit: function (response, postdata) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Failed to get result from server.", null];
            }
        };

        var deleteOptions = {
            url: '/Admin/DeleteCategory',
            caption: 'Delete Category',
            processData: "Saving...",
            width: 500,
            msg: "Delete the category? This will delete all the posts belonged to this category as well.",
            closeOnEscape: true,
            afterSubmit: JustBlog.GridManager.afterSubmitHandler
        };

        // configuring the navigation toolbar.
        $(gridName).jqGrid('navGrid', pagerName, {
            cloneToTop: true,
            search: false
        },

        editOptions, addOptions, deleteOptions);
    };

    //************************* TAGS GRID
    JustBlog.GridManager.tagsGrid = function (gridName, pagerName) {
        var colNames = ['Id', 'Name', 'Url Slug', 'Description'];

        var columns = [];

        columns.push({
            name: 'Id',
            index: 'Id',
            hidden: true,
            sorttype: 'int',
            key: true,
            editable: false,
            editoptions: {
                readonly: true
            }
        });

        columns.push({
            name: 'Name',
            index: 'Name',
            width: 200,
            editable: true,
            edittype: 'text',
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'UrlSlug',
            index: 'UrlSlug',
            width: 200,
            editable: true,
            edittype: 'text',
            sortable: false,
            editoptions: {
                size: 30,
                maxlength: 50
            },
            editrules: {
                required: true
            }
        });

        columns.push({
            name: 'Description',
            index: 'Description',
            width: 200,
            editable: true,
            edittype: 'textarea',
            sortable: false,
            editoptions: {
                rows: "4",
                cols: "28"
            }
        });

        $(gridName).jqGrid({
            url: '/Admin/Tags',
            datatype: 'json',
            mtype: 'GET',
            height: 'auto',
            toppager: true,
            colNames: colNames,
            colModel: columns,
            pager: pagerName,
            rownumbers: true,
            rownumWidth: 40,
            rowNum: 500,
            sortname: 'Name',
            loadonce: true,
            jsonReader: {
                repeatitems: false
            }
        });

        var editOptions = {
            url: '/Admin/EditTag',
            editCaption: 'Edit Tag',
            processData: "Saving...",
            closeAfterEdit: true,
            closeOnEscape: true,
            width: 400,
            afterSubmit: function (response, postdata) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Failed to get result from server.", null];
            }
        };

        var addOptions = {
            url: '/Admin/AddTag',
            addCaption: 'Add Tag',
            processData: "Saving...",
            closeAfterAdd: true,
            closeOnEscape: true,
            width: 400,
            afterSubmit: function (response, postdata) {
                var json = $.parseJSON(response.responseText);

                if (json) {
                    $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                    return [json.success, json.message, json.id];
                }

                return [false, "Failed to get result from server.", null];
            }
        };

        var deleteOptions = {
            url: '/Admin/DeleteTag',
            caption: 'Delete Tag',
            processData: "Saving...",
            width: 500,
            msg: "Delete the tag? This will delete all the posts belonged to this tag as well.",
            closeOnEscape: true,
            afterSubmit: JustBlog.GridManager.afterSubmitHandler
        };

        // configuring the navigation toolbar.
        $(gridName).jqGrid('navGrid', pagerName, {
            cloneToTop: true,
            search: false
        },

        editOptions, addOptions, deleteOptions);
    };

    /////////////////// ews edw den yparxei

    JustBlog.GridManager.afterSubmitHandler = function (response, postdata) {
        var json = $.parseJSON(response.responseText);

        if (json) return [json.success, json.message, json.id];

        return [false, "Failed to get result from server.", null];

        //var data = $.parseJSON(response.responseText);

        //if (data.success == true) {
        //    if ($("#Image").val() != "") {
        //        ajaxFileUpload(data.id);
        //    }
        //}

        //return [data.success, data.message, data.id];
    };

    $("#tabs").tabs({
        show: function (event, ui) {
            if (!ui.tab.isLoaded) {
                var gdMgr = JustBlog.GridManager,
                        fn, gridName, pagerName;

                switch (ui.index) {
                    case 0:
                        fn = gdMgr.postsGrid;
                        gridName = "#tablePosts";
                        pagerName = "#pagerPosts";
                        break;
                    case 1:
                        fn = gdMgr.categoriesGrid;
                        gridName = "#tableCats";
                        pagerName = "#pagerCats";
                        break;
                    case 2:
                        fn = gdMgr.tagsGrid;
                        gridName = "#tableTags";
                        pagerName = "#pagerTags";
                        break;
                };

                fn(gridName, pagerName);
                ui.tab.isLoaded = true;
            }
        }
    });
});