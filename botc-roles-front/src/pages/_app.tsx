import Layout from "@/components/layout/Layout";
import "@/styles/globals.css";
import type { AppProps } from "next/app";
import { NextUIProvider, createTheme, useSSR } from "@nextui-org/react";
import { ThemeProvider as NextThemesProvider } from "next-themes";

export default function App({ Component, pageProps }: AppProps) {
  const { isBrowser } = useSSR();

  const lightTheme = createTheme({
    type: "dark",
    theme: {},
  });
  const darkTheme = createTheme({
    type: "dark",
    theme: {},
  });

  return (
    isBrowser && (
      <NextThemesProvider
        defaultTheme="system"
        attribute="class"
        value={{
          light: lightTheme.className,
          dark: darkTheme.className,
        }}
      >
        <NextUIProvider>
          <Layout>
            <Component {...pageProps} />
          </Layout>
        </NextUIProvider>
      </NextThemesProvider>
    )
  );
}
