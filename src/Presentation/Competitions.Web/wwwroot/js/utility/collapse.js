document.querySelectorAll(".sidebar__collapse").forEach(item=>{
    item.addEventListener("click",(e)=>{
        e.currentTarget.classList.toggle("open")
        const sibling=e.target.nextElementSibling
        if (sibling.style.maxHeight==="0px")
        sibling.style.maxHeight=`${sibling.scrollHeight}px`
        else  sibling.style.maxHeight="0px"
    })
})