import axios from "axios";
const api = "https://localhost:7161/api/"

export const UploadPfpApi = async (
    file: FormData,
) => {
    try {
        const data = await axios.patch(api + 's3/upload-pfp',
            {
                file
            },
            {
                withCredentials: true,
                headers: {
                    "Content-Type": "multipart/form-data", 
                }
            })
        return data
    }
    catch (err) {
        return err
    }
}