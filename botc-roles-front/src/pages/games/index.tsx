import { Fragment, useEffect, useState } from "react";
import { Game } from "@/entities/Game";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import { Link, Loading, Spacer, Text } from "@nextui-org/react";
import PlayerName from "@/components/ui/playerName";
import { DateToString } from "@/helper/date";
import { getAllGames } from "../../../data/back-api";

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

  function line(game: Game) {
    const pseudo =
      game.storyTeller.pseudo !== "" ? ` (${game.storyTeller.pseudo})` : "";

    return (
      <Link key={game.id} href={`/games/${game.id}`} color="text">
        <ListItem
          name={DateToString(game.datePlayed)}
          value={
            <Fragment>
              Contée par{" "}
              {
                <PlayerName
                  name={`${game.storyTeller.name}${pseudo}`}
                />
              }
            </Fragment>
          }
        ></ListItem>
      </Link>
    );
  }

  return (
    <Fragment>
      <Title>{title}</Title>
      <Container>
        {games.map(
          (game: Game) => line(game)
        )}
      </Container>
    </Fragment>
  );
}
