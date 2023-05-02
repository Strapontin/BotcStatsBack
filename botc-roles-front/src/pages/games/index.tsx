import { Fragment, useEffect, useState } from "react";
import { getAllGames } from "../../../data/dummy-backend";
import { Game } from "@/entities/Game";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import { Link, Loading, Spacer, Text } from "@nextui-org/react";
import PlayerName from "@/components/ui/playerName";

export default function GamesListPage() {
  const [games, setGames] = useState<Game[]>([]);
  const title = "Dernières parties jouées";

  useEffect(() => {
    async function initGames() {
      const p = await getAllGames();
      setGames(p);
    }
    initGames();
  }, []);

  if (games.length === 0) {
    return (
      <Fragment>
        <Title>{title}</Title>
        <Spacer y={3} />
        <Loading />
      </Fragment>
    );
  }

  console.log(games);

  return (
    <Fragment>
      <Title>{title}</Title>
      <Container>
        {games.map((game: any) => (
          <Link key={game.id} href={`/games/${game.id}`} color="text">
            <ListItem
              name={game.datePlayed}
              value={
                <Fragment>
                  Conté par {<PlayerName name={game.storyTeller} />}
                </Fragment>
              }
            ></ListItem>
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}
