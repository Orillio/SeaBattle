$(document).ready(function () {
    let buttons = $('.button_light');
    buttons.each((index, btn) => {
        $(btn).click(function(e) {
            if($(btn).hasClass('inactive')) return;

            let x = e.clientX - e.currentTarget.offsetLeft;
            let y = e.clientY - e.currentTarget.offsetTop;

            var span = document.createElement('span');
            span.classList.add('ripple');
            span.style.left = `${x}px`;
            span.style.top =`${y}px`;
            btn.appendChild(span);
            setTimeout(() => span.remove(), 600)
        });
    });
    
});