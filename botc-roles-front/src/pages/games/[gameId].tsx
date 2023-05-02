import { Fragment, useEffect, useState } from "react";
import { getGameById } from "../../../data/dummy-backend";
import { Game } from "@/entities/Game";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import PlayerName from "@/components/ui/playerName";
import DateUi from "@/components/ui/date-ui";
import ListItemLarge from "@/components/list-stats/ListItemLarge";
import { Link, Spacer } from "@nextui-org/react";
import RoleColored from "@/components/ui/role-colored";
import ImageIconName from "@/components/ui/image-icon-name";

export default function GamePage() {
  const gameId: number = Number(useRouter().query.gameId);
  const [game, setGame] = useState<Game>();

  useEffect(() => {
    async function initGame() {
      const p = await getGameById(gameId);
      setGame(p);
    }
    initGame();
  }, [gameId]);

  if (isNaN(gameId) || !game) {
    return <Title>Chargement {gameId}...</Title>;
  }

  const title = (
    <Title>
      Détails de la partie du <DateUi date={game.datePlayed} /> contée par{" "}
      <Link href={`/players/${game.storyTeller}`} color="text">
        <PlayerName name={game.storyTeller} />
      </Link>
    </Title>
  );

  return (
    <Fragment>
      {title}
      <Container>
        <ListItem name="Module" value={game.module} />
        <ListItem
          name="Conteur"
          value={<PlayerName name={game.storyTeller} />}
        />
        <ListItem
          name="Date de la partie"
          value={<DateUi date={game.datePlayed} />}
        />
        <ListItem name="Alignement gagnant" value={game.winningAlignment} />
        <ListItemLarge name="Notes" value={game.notes} />
        <Spacer y={2} />

        {game.playerRoleGame.map((prg) => (
          <Link
            key={prg.playerName}
            href={`/players/${prg.playerName}`}
            color="text"
          >
            <ListItem
              name={prg.playerName}
              value={
                <ImageIconName
                  name={prg.role.name}
                  category={prg.role.category}
                />
              }
            />
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}
