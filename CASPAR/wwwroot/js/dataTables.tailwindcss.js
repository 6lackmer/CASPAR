/*! DataTables Tailwind CSS integration
 */

/*
 * This is a tech preview of Tailwind CSS integration with DataTables.
 */

// Set the defaults for DataTables initialisation
$.extend(true, DataTable.defaults, {
	dom:
		"<'grid grid-cols-1 md:grid-cols-2'" +
		"<'self-center'l>" +
		"<'self-center place-self-end'f>" +
		"<'my-2 col-span-2 border border-neutral-200 rounded min-w-full bg-white'tr>" +
		"<'self-center'i>" +
		"<'self-center place-self-end'p>" +
		">",
	renderer: 'tailwindcss'
});


// Default class modification
$.extend(DataTable.ext.classes, {
	sWrapper: "dataTables_wrapper dt-tailwindcss",
	sFilterInput: "border placeholder-neutral-500 ml-2 px-3 py-2 rounded-lg border-neutral-200 focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50",
	sLengthSelect: "border px-3 py-2 rounded-lg border-neutral-200 focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50",
	sProcessing: "dt-processing uk-panel",
	tailwindcss: {
		paging: {
			active: 'font-semibold bg-neutral-100',
			notActive: 'bg-white',
			button: 'relative inline-flex justify-center items-center space-x-2 border px-4 py-2 -mr-px leading-6 hover:z-10 focus:z-10 active:z-10 border-neutral-200 active:border-neutral-200 active:shadow-none',
			first: 'rounded-l-lg',
			last: 'rounded-r-lg',
			enabled: 'text-neutral-800 hover:text-neutral-900 hover:border-neutral-300 hover:shadow-sm focus:ring focus:ring-neutral-300 focus:ring-opacity-25',
			notEnabled: 'text-neutral-300'
		},
		table: 'dataTable min-w-full text-sm align-middle whitespace-nowrap overflow-x-auto',
		thead: {
			row: 'border-b border-neutral-100',
			cell: 'px-3 py-4 text-neutral-900 bg-neutral-100/75 font-semibold text-left'
		},
		tbody: {
			row: 'even:bg-neutral-50',
			cell: 'p-3'
		},
		tfoot: {
			row: 'even:bg-neutral-50',
			cell: 'p-3 text-left'
		},
	}
});

// Abstract out DataTable's awful class property names were we can,
// others are handled below
DataTable.ext.classes.sTable = DataTable.ext.classes.tailwindcss.table;
DataTable.ext.classes.sStripeOdd = DataTable.ext.classes.tailwindcss.tbody.row;
DataTable.ext.classes.sStripeEven = DataTable.ext.classes.tailwindcss.tbody.row;
DataTable.ext.classes.sHeaderTH = DataTable.ext.classes.tailwindcss.thead.cell;
DataTable.ext.classes.sFooterTH = DataTable.ext.classes.tailwindcss.tfoot.cell;
DataTable.defaults.column.sClass = DataTable.ext.classes.tailwindcss.tbody.cell;

// Eventually the classes and styles to apply from above will be merged into
// DataTables core, but for now we add them in a Tailwind CSS specific file
// since this is the only one that uses them this way at the moment.
$(document).on('init.dt', function (e, settings) {
	let thead = settings.nTHead;
	let classes = DataTable.ext.classes.tailwindcss;

	$(thead).addClass(classes.thead.row);
});


/* UIkit paging button renderer */
DataTable.ext.renderer.pageButton.tailwindcss = function (settings, host, idx, buttons, page, pages) {
	var api = new DataTable.Api(settings);
	var classes = settings.oClasses.tailwindcss.paging;
	var lang = settings.oLanguage.oPaginate;
	var aria = settings.oLanguage.oAria.paginate || {};
	var btnDisplay;

	var attach = function (container, buttons) {
		var i, ien, node, button;
		var clickHandler = function (e) {
			e.preventDefault();
			if (!$(e.currentTarget).hasClass('disabled') && api.page() != e.data.action) {
				api.page(e.data.action).draw('page');
			}
		};

		for (i = 0, ien = buttons.length; i < ien; i++) {
			button = buttons[i];

			if (Array.isArray(button)) {
				attach(container, button);
			}
			else {
				let enabled = false;
				let active = false;

				btnDisplay = '';

				switch (button) {
					case 'ellipsis':
						btnDisplay = '&#x2026;';
						break;

					case 'first':
						btnDisplay = lang.sFirst;

						if (page > 0) {
							enabled = true;
						}
						break;

					case 'previous':
						btnDisplay = lang.sPrevious;

						if (page > 0) {
							enabled = true;
						}
						break;

					case 'next':
						btnDisplay = lang.sNext;

						if (page < pages - 1) {
							enabled = true;
						}
						break;

					case 'last':
						btnDisplay = lang.sLast;

						if (page < pages - 1) {
							enabled = true;
						}
						break;

					default:
						btnDisplay = button + 1;
						enabled = true;

						if (page === button) {
							active = true;
						}
						break;
				}

				if (btnDisplay) {
					var className = classes.button + ' ' +
						(active ? classes.active : classes.notActive) + ' ' +
						(enabled ? classes.enabled : classes.notEnabled);

					node = $('<a>', {
						'href': enabled ? '#' : null,
						'aria-controls': settings.sTableId,
						'aria-disabled': enabled ? null : 'true',
						'aria-label': aria[button],
						'role': 'link',
						'aria-current': active ? 'page' : null,
						'data-dt-idx': button,
						'tabindex': enabled ? settings.iTabIndex : -1,
						'class': className,
						'id': idx === 0 && typeof button === 'string' ?
							settings.sTableId + '_' + button :
							null
					})
						.html(btnDisplay)
						.appendTo(container);

					settings.oApi._fnBindAction(
						node, { action: button }, clickHandler
					);
				}
			}
		}
	};

	var hostEl = $(host);
	var activeEl = hostEl.find(document.activeElement).data('dt-idx');
	var paginationEl = hostEl.children('ul.pagination');

	if (paginationEl.length) {
		paginationEl.empty();
	}
	else {
		paginationEl = hostEl
			.html('<div class="text-center"/>')
			.children('div');
	}

	attach(
		paginationEl,
		buttons
	);

	paginationEl.children(':first-child').addClass(classes.first);
	paginationEl.children(':last-child').addClass(classes.last);

	if (activeEl !== undefined) {
		hostEl.find('[data-dt-idx=' + activeEl + ']').trigger('focus');
	}
};
