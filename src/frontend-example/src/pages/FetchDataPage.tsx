import {useApi} from "../hooks/useApi.tsx";
import {useEffect, useState} from "react";

export const FetchDataPage = () => {
  const api = useApi();
  const [data, setData] = useState<string>();

  useEffect(() => {
    api.get("/api/Story")
      .then((response) => {
        setData(JSON.stringify(response.data, null, 2));
      })
      .catch((error) => {
        setData(JSON.stringify(error, null, 2));
      });
  }, []);

  return (
    <>
      <h3>Data fetch example</h3>

        <pre style={{textAlign: "left"}}>
          {data ?? "Loading..."}
        </pre>
    </>
  )
}