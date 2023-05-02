/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  redirects() {
    return [
      {
        source: '/',
        destination: '/',
        permanent: true,
      },
    ]
  },
}

module.exports = nextConfig
