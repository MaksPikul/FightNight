import { Checkbox } from "../ui/checkbox";

interface PasswordChecksProps {
    password: string
}

export const PasswordChecks = ({
    password
}:PasswordChecksProps) => {
    const requirements = [
        { label: 'At least 6 characters', isValid: password.length >= 6 },
        { label: 'At least one lowercase letter', isValid: /[a-z]/.test(password) },
        { label: 'At least one uppercase letter', isValid: /[A-Z]/.test(password) },
        { label: 'At least one number', isValid: /[0-9]/.test(password) },
        { label: 'At least one special character - @$!%*?&', isValid: /[@$!%*?&]/.test(password) },
      ];

      return(
        <ul>
            {requirements.map((req, index)=>(
                <li
                className="text-sm flex gap-x-1 items-center"
                key={index}>
                    <Checkbox
                    className="size-sm" 
                    checked={req.isValid}
                    id={req.label}/>
                    {req.label}
                </li>
            ))}
        </ul>
      )
}