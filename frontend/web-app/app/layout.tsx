import type { Metadata } from "next";
import "./globals.css";
import NavBar from "./nav/NavBar";
import ToasterProvider from "./providers/ToasterProvider";
export const metadata: Metadata = {
  title: "Carsties app",
  description: "Auction app made to learn .NET microservices and NextJS",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <ToasterProvider />
        <NavBar />
        <main className="container mx-auto px-5 pt-10">
            {children}
        </main>
        
      </body>
    </html>
  );
}
