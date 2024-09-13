"use client"

import { useTheme } from "next-themes"
import { Moon, Sun, SunMoon} from "lucide-react"
import { useEffect, useState } from "react"

export const ModeToggle = () => {
    const [mounted, setMounted] = useState(false)
    const { theme, setTheme } = useTheme()
    
    useEffect(()=>{
        setMounted(true)
    }, [])

    if (!mounted) {
        return (
        <div className="p-2">
            <SunMoon  className="size-5"/>
        </div>
        )
    }

    const changeTheme = () => {
        if (theme === 'dark'){
            setTheme('light')
        }
        else {
            setTheme('dark')
        }
    }

    return(
        
        <button 
        className="hover:bg-slate-200 hover:text-indigo-800 p-2  rounded-md" 
        onClick={()=>changeTheme()}>
            {theme === 'dark' ? <Moon className="size-5"/> : <Sun className="size-5"/>}     
        </button>
        
    )
}