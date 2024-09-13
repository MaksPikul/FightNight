import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './global.css'
import 'croppie/croppie.css';
import { BrowserRouter } from 'react-router-dom'
import { UserProvider } from './Context/UseAuth.tsx'
import { ModalProvider } from './Context/UseModal';
import { TooltipProvider } from './Components/ui/tooltip';
import { Toaster } from './Components/ui/toaster.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter >
            <UserProvider>
                <TooltipProvider>
                    <ModalProvider />
                    <App />
                    <Toaster />
                </TooltipProvider>
            </UserProvider >
        </BrowserRouter>
  </StrictMode>,
)
