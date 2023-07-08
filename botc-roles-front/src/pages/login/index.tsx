import { getLoginUrl } from "../../../data/back-api/back-api";

export default function IndexPage() {}

export async function getStaticProps() {
  const loginUrl = getLoginUrl();
  console.log(loginUrl);

  return {
    redirect: {
      destination: loginUrl,
      permanent: false,
      // statusCode: 301
    },
  };
}
