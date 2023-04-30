/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  redirects() {
    return [
      {
        source: '/',
        destination: '/parties-joueurs',
        permanent: true,
      },
    ]
  },
}

module.exports = nextConfig
