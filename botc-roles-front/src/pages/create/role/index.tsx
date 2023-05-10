import { Fragment, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Container, Input, Spacer } from "@nextui-org/react";
import DropdownCharacterType from "@/components/dropdown-character-type/DropdownCharacterType";
import DropdownAlignment from "@/components/dropdown-alignment/DropdownAlignment";

export default function CreateRole() {
  const [error, setError] = useState(<Fragment />);

  const title = <Title>Création d{"'"}un nouveau rôle</Title>;

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {error}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <Input
          clearable
          bordered
          labelPlaceholder="Nom"
          aria-label="Nom"
          // value={prenom}
          // onChange={(event) => setPrenom(event.target.value)}
        ></Input>
        <Spacer y={1.75} />
        <DropdownCharacterType />
        <Spacer y={1.75} />
        <DropdownAlignment />
        <Spacer y={3} />
      </Container>

      <Button
        shadow
        ghost
        color="success"
        // onPress={createUser}
        // disabled={!canCreatePlayer()}
      >
        Créer un rôle
      </Button>
      <Spacer y={3} />
    </Fragment>
  );
}
