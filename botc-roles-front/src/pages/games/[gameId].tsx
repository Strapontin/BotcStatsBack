import { Fragment, useEffect, useState } from "react";
import { Game } from "@/entities/Game";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import PlayerName from "@/components/ui/playerName";
import DateUi from "@/components/ui/date-ui";
import ListItemLarge from "@/components/list-stats/ListItemLarge";
import { Link, Spacer } from "@nextui-org/react";
import ImageIconName from "@/components/ui/image-icon-name";
import { getGameById } from "../../../data/back-api";
import { PlayerRoleGame } from "@/entities/PlayerRoleGame";
import { PlayerRole } from "@/entities/PlayerRole";
import { Alignment, alignmentToString } from "@/entities/enums/alignment";

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
      Détails de la partie du <DateUi date={game.creationDate} /> contée par{" "}
      <Link href={`/players/${game.storyTeller}`} color="text">
        <PlayerName name={game.storyTeller.name} />
      </Link>
    </Title>
  );

  return (
    <Fragment>
      {title}
      <Container>
        <ListItem name="Module" value={game.module.name} />
        <ListItem
          name="Conteur"
          value={<PlayerName name={game.storyTeller.name} />}
        />
        <ListItem
          name="Date de la partie"
          value={<DateUi date={game.creationDate} />}
        />
        <ListItem
          name="Alignement gagnant"
          value={alignmentToString(game.winningAlignment)}
        />
        <ListItemLarge name="Notes" value={game.notes} />
        <Spacer y={2} />

        {game.playerRoles.map((prg: PlayerRole) => (
          <Link
            key={prg.player.name}
            href={`/players/${prg.player.name}`}
            color="text"
          >
            <ListItem
              name={prg.player.name}
              value={
                <ImageIconName
                  name={prg.role.name}
                  characterType={prg.role.characterType}
                />
              }
            />
          </Link>
        ))}
      </Container>
    </Fragment>
  );
}
