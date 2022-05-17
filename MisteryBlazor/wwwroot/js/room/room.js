function ChangeClass(id, className) {
    document.getElementById(id).className = className;
}

function EnableContextMenu(menuId, handler, activeClass, inactiveClass) {
    const handleElement = document.getElementById(handler);
    handleElement.classList.add("m-context-menu");
    handleElement.addEventListener("contextmenu", function(event) {
        HandleMenu_contextmenu(event, menuId, activeClass, inactiveClass);
    });
    window.addEventListener("click", function(event) {
        HandleMenu_onclick(event, menuId, activeClass, inactiveClass);
    });
}

function DisableContextMenu(menuId, handler, activeClass, inactiveClass) {
    const handleElement = document.getElementById(handler);;
    handleElement.removeEventListener("contextmenu", function (event) {
        HandleMenu_contextmenu(event, menuId, activeClass, inactiveClass);
    });
    window.addEventListener("click", function (event) {
        HandleMenu_onclick(event, menuId, activeClass, inactiveClass);
    });
}

function HandleMenu_contextmenu(event, menuId, activeClass, inactiveClass) {
    event.preventDefault();
    const contextElement = document.getElementById(menuId);
    event.stopPropagation();
    contextElement.classList.remove(inactiveClass);
    contextElement.style.top = event.pageY + "px";
    contextElement.style.left = event.pageX + "px";
    contextElement.classList.add(activeClass);
}

function HandleMenu_onclick(event, menuId, activeClass, inactiveClass) {
    const contextElement = document.getElementById(menuId);
    if (contextElement) {
        event.stopPropagation();
        contextElement.classList.remove(activeClass);
        contextElement.classList.add(inactiveClass);
    }
}