import { Fragment, useEffect, useState } from "react";
import { Player } from "@/entities/Player";
import { useRouter } from "next/router";
import Container from "@/components/list-stats/Container";
import ListItem from "@/components/list-stats/ListItem";
import Title from "@/components/ui/title";
import PlayerName from "@/components/ui/playerName";
import { Collapse, Loading, Spacer } from "@nextui-org/react";
import { getPlayerByName } from "../../../data/back-api";
import ListItemRole from "@/components/list-stats/ListItemRole";

export default function PlayerPage() {
  const playerName = useRouter().query.playerName?.toString();
  const [player, setPlayer] = useState<Player>();

  useEffect(() => {
    async function initPlayer() {
      if (playerName === undefined) return;

      const p = await getPlayerByName(playerName);
      if (p !== undefined) {
        setPlayer(p);
      }
    }
    initPlayer();
  }, [playerName]);

  if (!playerName) {
    <Loading />;
    return;
  }

  const title = (
    <Title>
      Détails <PlayerName name={playerName} />
    </Title>
  );

  const playerComponent = player ? (
    <Collapse expanded title="Détails généraux">
      <Container>
        <ListItem name="Parties jouées" value={player.nbGamesPlayed} />
        <ListItem name="Parties gagnées | perdues" value={player.nbGamesWon} />
        <ListItem name="Parties perdues" value={player.nbGamesLost} />
        <ListItem name="Parties gentil" value={player.nbGamesGood} />
        <ListItem name="Parties maléfique" value={player.nbGamesEvil} />
      </Container>
    </Collapse>
  ) : (
    <Fragment />
  );

  const rolesComponent = player?.timesPlayedRole ? (
    <Collapse expanded title="Détails des rôles joués">
      <Container>
        {player.timesPlayedRole.map((tpr) => (
          <ListItemRole
            key={tpr.name}
            image={tpr.name}
            characterType={tpr.characterType}
            nbWins={tpr.timesWonByPlayer}
            nbLoses={tpr.timesLostByPlayer}
            nbGamesPlayed={tpr.timesPlayedByPlayer}
          />
        ))}
      </Container>
    </Collapse>
  ) : (
    <Loading />
  );

  return (
    <Fragment>
      {title}
      <Collapse.Group accordion={false} css={{ w: "100%" }}>
        {playerComponent}
        {rolesComponent}
      </Collapse.Group>
    </Fragment>
  );
}
